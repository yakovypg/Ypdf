#!/usr/bin/env bash

set -euo pipefail

if [ $# -lt 1 ]; then
  echo "error: path to the executable file is not specified" >&2
  exit 1
fi

EXECUTABLE="$1"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
INPUT_DIR="$SCRIPT_DIR/Input"
OUTPUT_DIR="$SCRIPT_DIR/Output"

HELP_OUTPUT="$OUTPUT_DIR/output_help.txt"
TOOL_HELP_OUTPUT="$OUTPUT_DIR/output_tool_help.txt"
VERSION_OUTPUT="$OUTPUT_DIR/output_version.txt"

SHOW_CONFIG_OUTPUT="$OUTPUT_DIR/output_show_config.txt"
SAVE_CONFIG_OUTPUT="$OUTPUT_DIR/output_save_config.txt"
RESET_CONFIG_OUTPUT="$OUTPUT_DIR/output_reset_config.txt"

SPLIT_BY_RANGES_INPUT="$INPUT_DIR/input_split.pdf"
SPLIT_BY_RANGES_OUTPUT="$OUTPUT_DIR/output_split_by_ranges"
SPLIT_BY_DEFAULT_SIZE_INPUT="$INPUT_DIR/input_split_large.pdf"
SPLIT_BY_DEFAULT_SIZE_OUTPUT="$OUTPUT_DIR/output_split_by_default_size"
SPLIT_BY_CUSTOM_SIZE_INPUT="$INPUT_DIR/input_split_large.pdf"
SPLIT_BY_CUSTOM_SIZE_OUTPUT="$OUTPUT_DIR/output_split_by_custom_size"

check_output_lines_greater_than() {
  local output_file_path="$1"
  local value="$2"
  local lines=$(wc -l < "$output_file_path" || echo 0)

  if [ "$lines" -gt "$value" ]; then
    return 0
  fi

  return 1
}

check_output_lines_equal_to() {
  local output_file_path="$1"
  local value="$2"
  local lines=$(wc -l < "$output_file_path" || echo 0)

  if [ "$lines" -eq "$value" ]; then
    return 0
  fi

  return 1
}

test_help() {
  if ! "$EXECUTABLE" --help > "$HELP_OUTPUT" 2>&1; then
    return 1
  fi

  check_output_lines_greater_than "$HELP_OUTPUT" 0
  return $?
}

test_tool_help() {
  if ! "$EXECUTABLE" merge --help > "$TOOL_HELP_OUTPUT" 2>&1; then
    return 1
  fi

  check_output_lines_greater_than "$TOOL_HELP_OUTPUT" 0
  return $?
}

test_version() {
  if ! "$EXECUTABLE" --version > "$VERSION_OUTPUT" 2>&1; then
    return 1
  fi

  check_output_lines_equal_to "$VERSION_OUTPUT" 1
  return $?
}

test_show_config() {
  if ! "$EXECUTABLE" config --show > "$SHOW_CONFIG_OUTPUT" 2>&1; then
    return 1
  fi

  check_output_lines_greater_than "$SHOW_CONFIG_OUTPUT" 0
  return $?
}

test_save_config() {
  if ! "$EXECUTABLE" config --save \
    --python-alias python3 \
    --python-venv "/home/$USER/.ypdf-venv" > "$SAVE_CONFIG_OUTPUT" 2>&1; then

    return 1
  fi

  check_output_lines_equal_to "$SAVE_CONFIG_OUTPUT" 0
  return $?
}

test_reset_config() {
  if ! "$EXECUTABLE" config --reset > "$RESET_CONFIG_OUTPUT" 2>&1; then
    return 1
  fi

  check_output_lines_equal_to "$RESET_CONFIG_OUTPUT" 0
  return $?
}

test_split_by_ranges() {
  if ! "$EXECUTABLE" -y split -i "$SPLIT_BY_RANGES_INPUT" -o "$SPLIT_BY_RANGES_OUTPUT" -p 1-3 6 8-10; then
    return 1
  fi

  return 0
}

test_split_by_default_size() {
  if ! "$EXECUTABLE" -y split -i "$SPLIT_BY_DEFAULT_SIZE_INPUT" -o "$SPLIT_BY_DEFAULT_SIZE_OUTPUT"; then
    return 1
  fi

  return 0
}

test_split_by_custom_size() {
  if ! "$EXECUTABLE" -y split -i "$SPLIT_BY_CUSTOM_SIZE_INPUT" -o "$SPLIT_BY_CUSTOM_SIZE_OUTPUT" --part-size "10*1024*1024"; then
    return 1
  fi

  return 0
}

print_tests_summary() {
  local passed_tests=$1
  local failed_tests=$2

  shift 2
  local failed_test_names=("$@")

  if [ "${#failed_test_names[@]}" -gt 0 ]; then
    echo ""
    echo "Failed test names:"

    for failed_test_name in "${failed_test_names[@]}"; do
      echo "$failed_test_name"
    done
  fi

  echo ""
  echo "=== Summary ==="
  printf "Passed tests: %d\n" "$passed_tests"
  printf "Failed tests: %d\n" "$failed_tests"
}

run_tests() {
  local test_functions=("$@")

  local passed_tests=0
  local failed_tests=0
  local failed_test_names=()

  local current_test_num=1
  local tests_count="${#test_functions[@]}"

  for test_func in "${test_functions[@]}"; do
    if "$test_func"; then
      passed_tests=$((passed_tests + 1))
      printf "PASS: %s (%s/%s)\n" "$test_func" "$current_test_num" "$tests_count"
    else
      failed_tests=$((failed_tests + 1))
      failed_test_names+=("$test_func")
      printf "FAIL: %s (%s/%s)\n" "$test_func" "$current_test_num" "$tests_count"
    fi

    current_test_num=$((current_test_num + 1))
  done

  print_tests_summary "$passed_tests" "$failed_tests" "${failed_test_names[@]}"

  if [ "$failed_tests" -gt 0 ]; then
    return 1
  fi

  return 0
}

create_dir_if_needed() {
  local dir="$1"

  if [ ! -d "$dir" ]; then
    mkdir -p "$dir"
  fi
}

prepare_dirs() {
  create_dir_if_needed "$OUTPUT_DIR"
}

TEST_FUNCTIONS=(
  test_help
  test_tool_help
  test_version
  test_show_config
  test_save_config
  test_reset_config
  test_split_by_ranges
  test_split_by_default_size
  test_split_by_custom_size
)

prepare_dirs
run_tests "${TEST_FUNCTIONS[@]}"
