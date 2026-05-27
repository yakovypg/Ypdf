# Tests for Ypdf.CommandLine

Integration tests are provided for **Ypdf.CommandLine**. They run the compiled executable via `bash` and perform basic verification of the output files.

Follow these steps to run the tests:
1. Build the project: `dotnet build ..`
2. Remove any previous test outputs (if present): `rm -rf ./Output`
3. Ensure an `Input` directory exists and contains files required by the tests inside it: `mkdir ./Input`
4. Run the test script: `./run_tests.sh`
