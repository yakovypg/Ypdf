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

CUSTOM_FONT_PATH="$SCRIPT_DIR/Input/Roboto-Bold.ttf"

HELP_OUTPUT="$OUTPUT_DIR/help.txt"
TOOL_HELP_OUTPUT="$OUTPUT_DIR/tool_help.txt"
VERSION_OUTPUT="$OUTPUT_DIR/version.txt"

CONFIG_SHOW_OUTPUT="$OUTPUT_DIR/config_show.txt"
CONFIG_SAVE_OUTPUT="$OUTPUT_DIR/config_save.txt"
CONFIG_RESET_OUTPUT="$OUTPUT_DIR/config_reset.txt"

SPLIT_BY_RANGES_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
SPLIT_BY_RANGES_OUTPUT="$OUTPUT_DIR/split_by_ranges"
SPLIT_BY_DEFAULT_SIZE_INPUT="$INPUT_DIR/pdf_60pages_76mb.pdf"
SPLIT_BY_DEFAULT_SIZE_OUTPUT="$OUTPUT_DIR/split_by_default_size"
SPLIT_BY_CUSTOM_SIZE_INPUT="$INPUT_DIR/pdf_60pages_76mb.pdf"
SPLIT_BY_CUSTOM_SIZE_OUTPUT="$OUTPUT_DIR/split_by_custom_size"

MERGE_TWO_FILES_INPUT_1="$INPUT_DIR/pdf_to_merge_1.pdf"
MERGE_TWO_FILES_INPUT_2="$INPUT_DIR/pdf_to_merge_2.pdf"
MERGE_TWO_FILES_OUTPUT="$OUTPUT_DIR/merge_two_files.pdf"
MERGE_MANY_FILES_INPUT_PREFIX="$INPUT_DIR/pdf_to_merge"
MERGE_MANY_FILES_OUTPUT="$OUTPUT_DIR/merge_many_files.pdf"

COMPRESS_DEFAULT_INPUT="$INPUT_DIR/pdf_to_compress.pdf"
COMPRESS_DEFAULT_OUTPUT="$OUTPUT_DIR/compress_default.pdf"
COMPRESS_CUSTOM_INPUT="$INPUT_DIR/pdf_to_compress.pdf"
COMPRESS_CUSTOM_OUTPUT="$OUTPUT_DIR/compress_custom.pdf"
COMPRESS_WITHOUT_VALIDITY_CHECK_INPUT="$INPUT_DIR/pdf_to_compress.pdf"
COMPRESS_WITHOUT_VALIDITY_CHECK_OUTPUT="$OUTPUT_DIR/compress_without_validity_check.pdf"
CAN_COMPRESS_TRUE_INPUT="$INPUT_DIR/pdf_to_compress.pdf"
CAN_COMPRESS_TRUE_OUTPUT="$OUTPUT_DIR/can_compress_true.txt"
CAN_COMPRESS_FALSE_INPUT="$INPUT_DIR/pdf_cannot_compress.pdf"
CAN_COMPRESS_FALSE_OUTPUT="$OUTPUT_DIR/can_compress_false.txt"

COPY_INPUT="$INPUT_DIR/pdf_10pages_blue.pdf"
COPY_OUTPUT="$OUTPUT_DIR/copy.pdf"

REMOVE_PAGES_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
REMOVE_PAGES_OUTPUT="$OUTPUT_DIR/remove_pages.pdf"

MOVE_PAGE_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
MOVE_PAGE_OUTPUT="$OUTPUT_DIR/move_page.pdf"

REORDER_PAGES_INPUT="$INPUT_DIR/pdf_5pages_blue.pdf"
REORDER_PAGES_OUTPUT="$OUTPUT_DIR/reorder_pages.pdf"

ROTATE_ALL_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
ROTATE_ALL_OUTPUT="$OUTPUT_DIR/rotate_all.pdf"
ROTATE_CUSTOM_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
ROTATE_CUSTOM_OUTPUT="$OUTPUT_DIR/rotate_custom.pdf"

CROP_INPUT="$INPUT_DIR/pdf_rectangles_1.pdf"
CROP_OUTPUT="$OUTPUT_DIR/crop.pdf"

DIVIDE_INPUT="$INPUT_DIR/pdf_rectangles_1.pdf"
DIVIDE_OUTPUT="$OUTPUT_DIR/divide.pdf"

ADD_PAGE_NUMBERS_DEFAULT_INPUT="$INPUT_DIR/pdf_10pages_blue.pdf"
ADD_PAGE_NUMBERS_DEFAULT_OUTPUT="$OUTPUT_DIR/add_page_numbers_default.pdf"
ADD_PAGE_NUMBERS_CUSTOM_INPUT="$INPUT_DIR/pdf_10pages_blue.pdf"
ADD_PAGE_NUMBERS_CUSTOM_OUTPUT="$OUTPUT_DIR/add_page_numbers_custom.pdf"
ADD_PAGE_NUMBERS_WITH_FONT_INPUT="$INPUT_DIR/pdf_10pages_blue.pdf"
ADD_PAGE_NUMBERS_WITH_FONT_OUTPUT="$OUTPUT_DIR/add_page_numbers_with_font.pdf"
ADD_PAGE_NUMBERS_WITH_CUSTOM_FONT_INPUT="$INPUT_DIR/pdf_10pages_blue.pdf"
ADD_PAGE_NUMBERS_WITH_CUSTOM_FONT_OUTPUT="$OUTPUT_DIR/add_page_numbers_with_custom_font.pdf"
ADD_PAGE_NUMBERS_WITH_PAGE_INCREASE_INPUT="$INPUT_DIR/pdf_10pages_blue.pdf"
ADD_PAGE_NUMBERS_WITH_PAGE_INCREASE_OUTPUT="$OUTPUT_DIR/add_page_numbers_with_page_increase.pdf"
ADD_PAGE_NUMBERS_WITH_CONTENT_SHIFT_INPUT="$INPUT_DIR/pdf_10pages_blue.pdf"
ADD_PAGE_NUMBERS_WITH_CONTENT_SHIFT_OUTPUT="$OUTPUT_DIR/add_page_numbers_with_content_shift.pdf"

ADD_WATERMARK_DEFAULT_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
ADD_WATERMARK_DEFAULT_OUTPUT="$OUTPUT_DIR/add_watermark_default.pdf"
ADD_WATERMARK_CUSTOM_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
ADD_WATERMARK_CUSTOM_OUTPUT="$OUTPUT_DIR/add_watermark_custom.pdf"
ADD_WATERMARK_WITH_FONT_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
ADD_WATERMARK_WITH_FONT_OUTPUT="$OUTPUT_DIR/add_watermark_with_font.pdf"
ADD_WATERMARK_WITH_CUSTOM_FONT_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
ADD_WATERMARK_WITH_CUSTOM_FONT_OUTPUT="$OUTPUT_DIR/add_watermark_with_custom_font.pdf"
ADD_WATERMARK_WITH_BORDER_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
ADD_WATERMARK_WITH_BORDER_OUTPUT="$OUTPUT_DIR/add_watermark_with_border.pdf"
ADD_WATERMARK_WITH_DIMENSIONS_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
ADD_WATERMARK_WITH_DIMENSIONS_OUTPUT="$OUTPUT_DIR/add_watermark_with_dimensions.pdf"
ADD_WATERMARK_ANNOTATION_INPUT="$INPUT_DIR/pdf_14pages_colored.pdf"
ADD_WATERMARK_ANNOTATION_OUTPUT="$OUTPUT_DIR/add_watermark_annotation.pdf"

REMOVE_WATERMARK_ANNOTATION_INPUT="$INPUT_DIR/pdf_watermark_annotation.pdf"
REMOVE_WATERMARK_ANNOTATION_OUTPUT="$OUTPUT_DIR/remove_watermark_annotation.pdf"

IMAGE_TO_PDF_TWO_FILES_INPUT_1="$INPUT_DIR/picture_1.png"
IMAGE_TO_PDF_TWO_FILES_INPUT_2="$INPUT_DIR/picture_2.bmp"
IMAGE_TO_PDF_TWO_FILES_OUTPUT="$OUTPUT_DIR/image_to_pdf_two_files.pdf"
IMAGE_TO_PDF_MANY_FILES_INPUT_PREFIX="$INPUT_DIR/picture"
IMAGE_TO_PDF_MANY_FILES_OUTPUT="$OUTPUT_DIR/image_to_pdf_many_files.pdf"
IMAGE_TO_PDF_CUSTOM_INPUT_1="$INPUT_DIR/picture_1.png"
IMAGE_TO_PDF_CUSTOM_INPUT_2="$INPUT_DIR/picture_2.bmp"
IMAGE_TO_PDF_CUSTOM_OUTPUT="$OUTPUT_DIR/image_to_pdf_custom.pdf"
IMAGE_TO_PDF_WITHOUT_AUTOINCREASE_INPUT_1="$INPUT_DIR/picture_1.png"
IMAGE_TO_PDF_WITHOUT_AUTOINCREASE_INPUT_2="$INPUT_DIR/picture_2.bmp"
IMAGE_TO_PDF_WITHOUT_AUTOINCREASE_OUTPUT="$OUTPUT_DIR/image_to_pdf_without_autoincrease.pdf"

TEXT_TO_PDF_DEFAULT_INPUT="$INPUT_DIR/text_multi.txt"
TEXT_TO_PDF_DEFAULT_OUTPUT="$OUTPUT_DIR/text_to_pdf_default.pdf"
TEXT_TO_PDF_CUSTOM_INPUT="$INPUT_DIR/text_multi.txt"
TEXT_TO_PDF_CUSTOM_OUTPUT="$OUTPUT_DIR/text_to_pdf_custom.pdf"
TEXT_TO_PDF_WITH_FONT_INPUT="$INPUT_DIR/text_multi.txt"
TEXT_TO_PDF_WITH_FONT_OUTPUT="$OUTPUT_DIR/text_to_pdf_with_font.pdf"
TEXT_TO_PDF_WITH_CUSTOM_FONT_INPUT="$INPUT_DIR/text_multi.txt"
TEXT_TO_PDF_WITH_CUSTOM_FONT_OUTPUT="$OUTPUT_DIR/text_to_pdf_with_custom_font.pdf"

EXTRACT_IMAGES_INPUT="$INPUT_DIR/pdf_with_images_and_text.pdf"
EXTRACT_IMAGES_OUTPUT="$OUTPUT_DIR/extract_images"

EXTRACT_TEXT_DEFAULT_INPUT="$INPUT_DIR/pdf_with_images_and_text.pdf"
EXTRACT_TEXT_DEFAULT_OUTPUT="$OUTPUT_DIR/extract_text_default.txt"
EXTRACT_TEXT_WITH_TIKA_INPUT="$INPUT_DIR/pdf_with_images_and_text.pdf"
EXTRACT_TEXT_WITH_TIKA_OUTPUT="$OUTPUT_DIR/extract_text_with_tika.txt"

SET_PASSWORD_SAME_INPUT="$INPUT_DIR/pdf_5pages_blue.pdf"
SET_PASSWORD_SAME_OUTPUT="$OUTPUT_DIR/set_password_same.pdf"
SET_PASSWORD_SEPARATE_INPUT="$INPUT_DIR/pdf_5pages_blue.pdf"
SET_PASSWORD_SEPARATE_OUTPUT="$OUTPUT_DIR/set_password_separate.pdf"
SET_PASSWORD_WITH_ENCRYPTION_INPUT="$INPUT_DIR/pdf_5pages_blue.pdf"
SET_PASSWORD_WITH_ENCRYPTION_OUTPUT="$OUTPUT_DIR/set_password_swith_encryption.pdf"

REMOVE_PASSWORD_DEFAULT_INPUT="$INPUT_DIR/pdf_with_password_same.pdf"
REMOVE_PASSWORD_DEFAULT_OUTPUT="$OUTPUT_DIR/remove_password_default.pdf"
REMOVE_PASSWORD_TRY_BOTH_INPUT="$INPUT_DIR/pdf_with_password_different.pdf"
REMOVE_PASSWORD_TRY_BOTH_OUTPUT="$OUTPUT_DIR/remove_password_try_both.pdf"

COMPRESS_IMAGES_SINGLE_INPUT="$INPUT_DIR/img_to_compress_2mb_1.jpg"
COMPRESS_IMAGES_SINGLE_OUTPUT="$OUTPUT_DIR/compress_images_single.jpg"
COMPRESS_IMAGES_MANY_FILES_INPUT_PREFIX="$INPUT_DIR/img_to_compress"
COMPRESS_IMAGES_MANY_FILES_OUTPUT="$OUTPUT_DIR/compress_images_many_files"
COMPRESS_IMAGES_WITH_SIZE_FACTOR_INPUT="$INPUT_DIR/img_to_compress_2mb_3.jpg"
COMPRESS_IMAGES_WITH_SIZE_FACTOR_OUTPUT="$OUTPUT_DIR/compress_images_with_size_factor.jpg"
COMPRESS_IMAGES_WITH_EXACT_SIZE_INPUT_PREFIX="$INPUT_DIR/img_to_compress_3mb_2.jpg"
COMPRESS_IMAGES_WITH_EXACT_SIZE_OUTPUT="$OUTPUT_DIR/compress_images_with_exact_size.jpg"

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

check_output_size_less_than_input_size() {
  local input_file_path="$1"
  local output_file_path="$2"

  local input_file_size=$(wc -c < "$input_file_path")
  local output_file_size=$(wc -c < "$output_file_path")

  if [ "$output_file_size" -lt "$input_file_size" ]; then
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

test_config_show() {
  if ! "$EXECUTABLE" config --show > "$CONFIG_SHOW_OUTPUT" 2>&1; then
    return 1
  fi

  check_output_lines_greater_than "$CONFIG_SHOW_OUTPUT" 0
  return $?
}

test_config_save() {
  if ! "$EXECUTABLE" config --save \
    --python-alias python3 \
    --python-venv "/home/$USER/.ypdf-venv" > "$CONFIG_SAVE_OUTPUT" 2>&1; then

    return 1
  fi

  check_output_lines_equal_to "$CONFIG_SAVE_OUTPUT" 0
  return $?
}

test_config_reset() {
  if ! "$EXECUTABLE" config --reset > "$CONFIG_RESET_OUTPUT" 2>&1; then
    return 1
  fi

  check_output_lines_equal_to "$CONFIG_RESET_OUTPUT" 0
  return $?
}

test_split_by_ranges() {
  if ! "$EXECUTABLE" -y split \
    -i "$SPLIT_BY_RANGES_INPUT" \
    -o "$SPLIT_BY_RANGES_OUTPUT" \
    -p 1-3 6 8-10; then

    return 1
  fi

  return 0
}

test_split_by_default_size() {
  if ! "$EXECUTABLE" -y split \
    -i "$SPLIT_BY_DEFAULT_SIZE_INPUT" \
    -o "$SPLIT_BY_DEFAULT_SIZE_OUTPUT"; then

    return 1
  fi

  return 0
}

test_split_by_custom_size() {
  if ! "$EXECUTABLE" -y split \
    -i "$SPLIT_BY_CUSTOM_SIZE_INPUT" \
    -o "$SPLIT_BY_CUSTOM_SIZE_OUTPUT" \
    --part-size "10*1024*1024"; then

    return 1
  fi

  return 0
}

test_merge_two_files() {
  if ! "$EXECUTABLE" -y merge \
    -i "$MERGE_TWO_FILES_INPUT_1" "$MERGE_TWO_FILES_INPUT_2" \
    -o "$MERGE_TWO_FILES_OUTPUT"; then

    return 1
  fi

  return 0
}

test_merge_many_files() {
  if ! "$EXECUTABLE" -y merge \
    -i "$MERGE_MANY_FILES_INPUT_PREFIX"*.pdf \
    -o "$MERGE_MANY_FILES_OUTPUT"; then

    return 1
  fi

  return 0
}

test_compress_default() {
  if ! "$EXECUTABLE" -y compress \
    -i "$COMPRESS_DEFAULT_INPUT" \
    -o "$COMPRESS_DEFAULT_OUTPUT"; then

    return 1
  fi

  check_output_size_less_than_input_size "$COMPRESS_DEFAULT_INPUT" "$COMPRESS_DEFAULT_OUTPUT"
  return $?
}

test_compress_custom() {
  if ! "$EXECUTABLE" -y compress \
    -i "$COMPRESS_CUSTOM_INPUT" \
    -o "$COMPRESS_CUSTOM_OUTPUT" \
    -e png \
    -q 0.5 \
    -s 0.8; then

    return 1
  fi

  #check_output_size_less_than_input_size "$COMPRESS_DEFAULT_INPUT" "$COMPRESS_DEFAULT_OUTPUT"
  return $?
}

test_compress_without_validity_check() {
  if ! "$EXECUTABLE" -y compress \
    -i "$COMPRESS_WITHOUT_VALIDITY_CHECK_INPUT" \
    -o "$COMPRESS_WITHOUT_VALIDITY_CHECK_OUTPUT" \
    --disable-compression-validity-check; then

    return 1
  fi

  check_output_size_less_than_input_size "$COMPRESS_DEFAULT_INPUT" "$COMPRESS_DEFAULT_OUTPUT"
  return $?
}

test_can_compress_true() {
  if ! "$EXECUTABLE" -y can-compress \
    -i "$CAN_COMPRESS_TRUE_INPUT" \
    -o "$CAN_COMPRESS_TRUE_OUTPUT"; then

    return 1
  fi

  if [[ "$(tr -d '\r\n' < "$CAN_COMPRESS_TRUE_OUTPUT")" != "True" ]]; then
    return 1
  fi

  return 0
}

test_can_compress_false() {
  if ! "$EXECUTABLE" -y can-compress \
    -i "$CAN_COMPRESS_FALSE_INPUT" \
    -o "$CAN_COMPRESS_FALSE_OUTPUT"; then

    return 1
  fi

  if [[ "$(tr -d '\r\n' < "$CAN_COMPRESS_FALSE_OUTPUT")" != "False" ]]; then
    return 1
  fi

  return 0
}

test_copy() {
  if ! "$EXECUTABLE" -y copy -i "$COPY_INPUT" -o "$COPY_OUTPUT"; then
    return 1
  fi

  return 0
}

test_remove_pages() {
  if ! "$EXECUTABLE" -y remove-pages \
    -i "$REMOVE_PAGES_INPUT" \
    -o "$REMOVE_PAGES_OUTPUT" \
    -p 1-3 5 7-10; then

    return 1
  fi

  return 0
}

test_move_page() {
  if ! "$EXECUTABLE" -y move-page \
    -i "$MOVE_PAGE_INPUT" \
    -o "$MOVE_PAGE_OUTPUT" \
    --from 5 \
    --to 10; then

    return 1
  fi

  return 0
}

test_reorder_pages() {
  if ! "$EXECUTABLE" -y reorder-pages \
    -i "$REORDER_PAGES_INPUT" \
    -o "$REORDER_PAGES_OUTPUT" \
    --page-order 5,3-1,4; then

    return 1
  fi

  return 0
}

test_rotate_all() {
  if ! "$EXECUTABLE" -y rotate -i "$ROTATE_ALL_INPUT" -o "$ROTATE_ALL_OUTPUT" -a 90; then
    return 1
  fi

  return 0
}

test_rotate_custom() {
  if ! "$EXECUTABLE" -y rotate -i "$ROTATE_CUSTOM_INPUT" -o "$ROTATE_CUSTOM_OUTPUT" -r 2:90 1,3-5:180; then
    return 1
  fi

  return 0
}

test_crop() {
  if ! "$EXECUTABLE" -y crop -i "$CROP_INPUT" -o "$CROP_OUTPUT" --cropping "1,3:(30;40),(60;60)"; then
    return 1
  fi

  return 0
}

test_divide() {
  if ! "$EXECUTABLE" -y divide \
    -i "$DIVIDE_INPUT" \
    -o "$DIVIDE_OUTPUT" \
    --division 1:horizontal 3-4:vertical,10; then

    return 1
  fi

  return 0
}

test_add_page_numbers_default() {
  if ! "$EXECUTABLE" -y add-page-numbers -i "$ADD_PAGE_NUMBERS_DEFAULT_INPUT" -o "$ADD_PAGE_NUMBERS_DEFAULT_OUTPUT"; then
    return 1
  fi

  return 0
}

test_add_page_numbers_custom() {
  if ! "$EXECUTABLE" -y add-page-numbers \
    -i "$ADD_PAGE_NUMBERS_CUSTOM_INPUT" \
    -o "$ADD_PAGE_NUMBERS_CUSTOM_OUTPUT" \
    --horizontal-alignment left \
    --vertical-alignment top \
    --num-presenter=verbal \
    --margin 0,10 \
    --left-page-margin \
    --top-page-margin \
    --right-page-margin \
    --bottom-page-margin; then
    return 1
  fi

  return 0
}

test_add_page_numbers_with_font() {
  if ! "$EXECUTABLE" -y add-page-numbers \
    -i "$ADD_PAGE_NUMBERS_WITH_FONT_INPUT" \
    -o "$ADD_PAGE_NUMBERS_WITH_FONT_OUTPUT" \
    --font-size 24 \
    --font-family Times-Bold \
    --font-color blue \
    --font-opacity 0.5; then
    return 1
  fi

  return 0
}

test_add_page_numbers_with_custom_font() {
  if ! "$EXECUTABLE" -y add-page-numbers \
    -i "$ADD_PAGE_NUMBERS_WITH_CUSTOM_FONT_INPUT" \
    -o "$ADD_PAGE_NUMBERS_WITH_CUSTOM_FONT_OUTPUT" \
    --font-size 24 \
    --font-path "$CUSTOM_FONT_PATH" \
    --font-encoding Identity-H \
    --font-color blue \
    --font-opacity 0.5; then
    return 1
  fi

  return 0
}

test_add_page_numbers_with_page_increase() {
  if ! "$EXECUTABLE" -y add-page-numbers \
    -i "$ADD_PAGE_NUMBERS_WITH_PAGE_INCREASE_INPUT" \
    -o "$ADD_PAGE_NUMBERS_WITH_PAGE_INCREASE_OUTPUT" \
    --increase-page-mode bottom \
    --fill-color green \
    --page-size-adjustment 40; then
    return 1
  fi

  return 0
}

test_add_page_numbers_with_content_shift() {
  if ! "$EXECUTABLE" -y add-page-numbers \
    -i "$ADD_PAGE_NUMBERS_WITH_CONTENT_SHIFT_INPUT" \
    -o "$ADD_PAGE_NUMBERS_WITH_CONTENT_SHIFT_OUTPUT" \
    --content-shift 1-10:-50,0; then
    return 1
  fi

  return 0
}

test_add_watermark_default() {
  if ! "$EXECUTABLE" -y add-watermark \
    -i "$ADD_WATERMARK_DEFAULT_INPUT" \
    -o "$ADD_WATERMARK_DEFAULT_OUTPUT" \
    --text "My watermark" \
    -p 1 5-10; then
    return 1
  fi

  return 0
}

test_add_watermark_custom() {
  if ! "$EXECUTABLE" -y add-watermark \
    -i "$ADD_WATERMARK_CUSTOM_INPUT" \
    -o "$ADD_WATERMARK_CUSTOM_OUTPUT" \
    --text "My watermark" \
    --angle 60 \
    --width 500 \
    --height 100 \
    --position "(250;250)" \
    --text-alignment left \
    --text-h-alignment left \
    --container-v-alignment bottom; then
    return 1
  fi

  return 0
}

test_add_watermark_with_font() {
  if ! "$EXECUTABLE" -y add-watermark \
    -i "$ADD_WATERMARK_WITH_FONT_INPUT" \
    -o "$ADD_WATERMARK_WITH_FONT_OUTPUT" \
    --text "My watermark" \
    --font-size 72 \
    --font-family Times-Bold \
    --font-color black \
    --font-opacity 0.5; then
    return 1
  fi

  return 0
}

test_add_watermark_with_custom_font() {
  if ! "$EXECUTABLE" -y add-watermark \
    -i "$ADD_WATERMARK_WITH_CUSTOM_FONT_INPUT" \
    -o "$ADD_WATERMARK_WITH_CUSTOM_FONT_OUTPUT" \
    --text "My watermark" \
    --font-size 72 \
    --font-path "$CUSTOM_FONT_PATH" \
    --font-encoding Identity-H \
    --font-color black \
    --font-opacity 0.5; then
    return 1
  fi

  return 0
}

test_add_watermark_with_border() {
  if ! "$EXECUTABLE" -y add-watermark \
    -i "$ADD_WATERMARK_WITH_BORDER_INPUT" \
    -o "$ADD_WATERMARK_WITH_BORDER_OUTPUT" \
    --text "My watermark" \
    --border dashed \
    --border-color blue \
    --border-thickness 5 \
    --border-opacity 0.7; then
    return 1
  fi

  return 0
}

test_add_watermark_with_dimensions() {
  if ! "$EXECUTABLE" -y add-watermark \
    -i "$ADD_WATERMARK_WITH_DIMENSIONS_INPUT" \
    -o "$ADD_WATERMARK_WITH_DIMENSIONS_OUTPUT" \
    -t "My watermark" \
    -a 60 \
    --position "(250;0)" \
    --width 10000 \
    --height 10000 \
    --text-alignment left \
    --text-h-alignment left \
    --container-v-alignment bottom; then
    return 1
  fi

  return 0
}

test_add_watermark_annotation() {
  if ! "$EXECUTABLE" -y add-watermark-annotation \
    -i "$ADD_WATERMARK_ANNOTATION_INPUT" \
    -o "$ADD_WATERMARK_ANNOTATION_OUTPUT" \
    -t "My watermark" \
    -a 60 \
    --width 300 \
    --height 450 \
    --x-translation 50 \
    --y-translation 25 \
    --font-size 72 \
    --font-family Times-Bold \
    --font-color black \
    --font-opacity 0.5; then
    return 1
  fi

  return 0
}

test_remove_watermark_annotation() {
  if ! "$EXECUTABLE" -y remove-watermark-annotation \
    -i "$REMOVE_WATERMARK_ANNOTATION_INPUT" \
    -o "$REMOVE_WATERMARK_ANNOTATION_OUTPUT" \
    -p 1 5-10; then
    return 1
  fi

  return 0
}

test_image_to_pdf_two_files() {
  if ! "$EXECUTABLE" -y image-to-pdf \
    -i "$IMAGE_TO_PDF_TWO_FILES_INPUT_1" "$IMAGE_TO_PDF_TWO_FILES_INPUT_2" \
    -o "$IMAGE_TO_PDF_TWO_FILES_OUTPUT"; then
    return 1
  fi

  return 0
}

test_image_to_pdf_many_files() {
  if ! "$EXECUTABLE" -y image-to-pdf \
    -i "$IMAGE_TO_PDF_MANY_FILES_INPUT_PREFIX"* \
    -o "$IMAGE_TO_PDF_MANY_FILES_OUTPUT"; then
    return 1
  fi

  return 0
}

test_image_to_pdf_custom() {
  if ! "$EXECUTABLE" -y image-to-pdf \
    -i "$IMAGE_TO_PDF_CUSTOM_INPUT_1" "$IMAGE_TO_PDF_CUSTOM_INPUT_2" \
    -o "$IMAGE_TO_PDF_CUSTOM_OUTPUT" \
    --page-size a4 \
    --margin 10 \
    --image-h-alignment left \
    --angle 90; then
    return 1
  fi

  return 0
}

test_image_to_pdf_without_autoincrease() {
  if ! "$EXECUTABLE" -y image-to-pdf \
    -i "$IMAGE_TO_PDF_WITHOUT_AUTOINCREASE_INPUT_1" "$IMAGE_TO_PDF_WITHOUT_AUTOINCREASE_INPUT_2" \
    -o "$IMAGE_TO_PDF_WITHOUT_AUTOINCREASE_OUTPUT" \
    --disable-autoincrease-size; then
    return 1
  fi

  return 0
}

test_text_to_pdf_default() {
  if ! "$EXECUTABLE" -y text-to-pdf \
    -i "$TEXT_TO_PDF_DEFAULT_INPUT" \
    -o "$TEXT_TO_PDF_DEFAULT_OUTPUT"; then

    return 1
  fi

  return 0
}

test_text_to_pdf_custom() {
  if ! "$EXECUTABLE" -y text-to-pdf \
    -i "$TEXT_TO_PDF_CUSTOM_INPUT" \
    -o "$TEXT_TO_PDF_CUSTOM_OUTPUT" \
    -m 50,100,50,100 \
    -a right \
    -s a4; then

    return 1
  fi

  return 0
}

test_text_to_pdf_with_font() {
  if ! "$EXECUTABLE" -y text-to-pdf \
    -i "$TEXT_TO_PDF_WITH_FONT_INPUT" \
    -o "$TEXT_TO_PDF_WITH_FONT_OUTPUT" \
    --font-size 72 \
    --font-family Times-Bold \
    --font-color black \
    --font-opacity 0.5; then

    return 1
  fi

  return 0
}

test_text_to_pdf_with_custom_font() {
  if ! "$EXECUTABLE" -y text-to-pdf \
    -i "$TEXT_TO_PDF_WITH_CUSTOM_FONT_INPUT" \
    -o "$TEXT_TO_PDF_WITH_CUSTOM_FONT_OUTPUT" \
    --font-size 72 \
    --font-path "$CUSTOM_FONT_PATH" \
    --font-encoding Identity-H \
    --font-color black \
    --font-opacity 0.5; then

    return 1
  fi

  return 0
}

test_extract_images() {
  if ! "$EXECUTABLE" -y extract-images \
    -i "$EXTRACT_IMAGES_INPUT" \
    -o "$EXTRACT_IMAGES_OUTPUT"; then

    return 1
  fi

  return 0
}

test_extract_text_default() {
  if ! "$EXECUTABLE" -y extract-text \
    -i "$EXTRACT_TEXT_DEFAULT_INPUT" \
    -o "$EXTRACT_TEXT_DEFAULT_OUTPUT"; then

    return 1
  fi

  return 0
}

test_extract_text_with_tika() {
  if ! "$EXECUTABLE" -y extract-text \
  -i "$EXTRACT_TEXT_WITH_TIKA_INPUT" \
  -o "$EXTRACT_TEXT_WITH_TIKA_OUTPUT" \
  --use-tika; then

    return 1
  fi

  return 0
}

test_set_password_same() {
  if ! "$EXECUTABLE" -y set-password \
    -i "$SET_PASSWORD_SAME_INPUT" \
    -o "$SET_PASSWORD_SAME_OUTPUT" \
    --password "samePassword"; then

    return 1
  fi

  return 0
}

test_set_password_separate() {
  if ! "$EXECUTABLE" -y set-password \
    -i "$SET_PASSWORD_SEPARATE_INPUT" \
    -o "$SET_PASSWORD_SEPARATE_OUTPUT" \
    --user-password "user" \
    --owner-password "admin"; then

    return 1
  fi

  return 0
}

test_set_password_with_encryption() {
  if ! "$EXECUTABLE" -y set-password \
    -i "$SET_PASSWORD_WITH_ENCRYPTION_INPUT" \
    -o "$SET_PASSWORD_WITH_ENCRYPTION_OUTPUT" \
    --password "samePassword" \
    -e encryption_aes_128; then

    return 1
  fi

  return 0
}

test_remove_password_default() {
  if ! "$EXECUTABLE" -y remove-password \
    -i "$REMOVE_PASSWORD_DEFAULT_INPUT" \
    -o "$REMOVE_PASSWORD_DEFAULT_OUTPUT" \
    --password "samePassword"; then

    return 1
  fi

  return 0
}

test_remove_password_try_both() {
  if ! "$EXECUTABLE" -y remove-password \
    -i "$REMOVE_PASSWORD_TRY_BOTH_INPUT" \
    -o "$REMOVE_PASSWORD_TRY_BOTH_OUTPUT" \
    --user-password "user" \
    --owner-password "admin"; then

    return 1
  fi

  return 0
}

test_compress_images_single() {
  if ! "$EXECUTABLE" -y compress-images \
    -i "$COMPRESS_IMAGES_SINGLE_INPUT" \
    -o "$COMPRESS_IMAGES_SINGLE_OUTPUT"; then
    return 1
  fi

  return 0
}

test_compress_images_many_files() {
  if ! "$EXECUTABLE" -y compress-images \
    -i "$COMPRESS_IMAGES_MANY_FILES_INPUT_PREFIX"* \
    -o "$COMPRESS_IMAGES_MANY_FILES_OUTPUT"; then
    return 1
  fi

  return 0
}

test_compress_images_with_size_factor() {
  if ! "$EXECUTABLE" -y compress-images \
    -i "$COMPRESS_IMAGES_WITH_SIZE_FACTOR_INPUT" \
    -o "$COMPRESS_IMAGES_WITH_SIZE_FACTOR_OUTPUT" \
    -q 0.75 \
    -s 0.7; then
    return 1
  fi

  return 0
}

test_compress_images_single_with_exact_size() {
  if ! "$EXECUTABLE" -y compress-images \
    -i "$COMPRESS_IMAGES_WITH_EXACT_SIZE_INPUT_PREFIX"* \
    -o "$COMPRESS_IMAGES_WITH_EXACT_SIZE_OUTPUT" \
    --quality-factor 0.75 \
    --width 1920 \
    --height 1080 \
    --extension jpg; then
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
  test_config_show
  test_config_save
  test_config_reset
  test_split_by_ranges
  test_split_by_default_size
  test_split_by_custom_size
  test_merge_two_files
  test_merge_many_files
  test_compress_default
  test_compress_custom
  test_compress_without_validity_check
  test_can_compress_true
  test_can_compress_false
  test_copy
  test_remove_pages
  test_move_page
  test_reorder_pages
  test_rotate_all
  test_rotate_custom
  test_crop
  test_divide
  test_add_page_numbers_default
  test_add_page_numbers_custom
  test_add_page_numbers_with_font
  test_add_page_numbers_with_custom_font
  test_add_page_numbers_with_page_increase
  test_add_page_numbers_with_content_shift
  test_add_watermark_default
  test_add_watermark_custom
  test_add_watermark_with_font
  test_add_watermark_with_custom_font
  test_add_watermark_with_border
  test_add_watermark_with_dimensions
  test_add_watermark_annotation
  test_remove_watermark_annotation
  test_image_to_pdf_two_files
  test_image_to_pdf_many_files
  test_image_to_pdf_custom
  test_image_to_pdf_without_autoincrease
  test_text_to_pdf_default
  test_text_to_pdf_custom
  test_text_to_pdf_with_font
  test_text_to_pdf_with_custom_font
  test_extract_images
  test_extract_text_default
  test_extract_text_with_tika
  test_set_password_same
  test_set_password_separate
  test_set_password_with_encryption
  test_remove_password_default
  test_remove_password_try_both
  test_compress_images_single
  test_compress_images_many_files
  test_compress_images_with_size_factor
  test_compress_images_single_with_exact_size
)

prepare_dirs
run_tests "${TEST_FUNCTIONS[@]}"
