#!/bin/bash

# We can't run emulator as a daemon
# VSTS will not execute next step until emulator killed
# So we need to run tests in same step...
export DYLD_LIBRARY_PATH="$ANDROID_HOME/emulator/lib64/qt/lib"
$ANDROID_HOME/emulator/emulator -avd emulator -skin 768x1280 -no-window -gpu off &

# Ensure Android Emulator has booted successfully before continuing
EMU_BOOTED='unknown'
while [[ ${EMU_BOOTED} != *"stopped"* ]]; do
    echo "Waiting emulator to start..."
    sleep 5
    EMU_BOOTED=`adb shell getprop init.svc.bootanim || echo unknown`
done
duration=$(( SECONDS - start ))
echo "Android Emulator started after $duration seconds."

# Listen to tests
nc -l 127.0.0.1 16384 | tee results.txt &
RESULTS=$!

# Run tests
adb install Tests/Contoso.Android.FuncTest/bin/Release/com.contoso.android.functest.apk
adb shell monkey -p com.contoso.android.functest -c android.intent.category.LAUNCHER 1

# Wait results
wait $RESULTS

# Check if at least 1 test ran and none failed
egrep "Tests run: [^0]" results.txt | egrep "Failed: 0" results.txt > /dev/null
TEST_PASSED=$?

# And kill emulator
adb emu kill

# Exit with test result code
exit $TEST_PASSED