# pip install tika==3.1.0

import sys
import argparse

from tika import parser

def print_error() -> None:
    exceprion_info = sys.exc_info()
    message = exceprion_info[1]
    print(message)

def extract_text(input_path: str, output_path: str) -> None:
    try:
        pdf_document = parser.from_file(input_path)
        content = pdf_document['content']

        file = open(output_path, 'w', encoding='utf-8')
        file.write(content)
    except:
        print_error()

if __name__ == "__main__":
    argParser = argparse.ArgumentParser(
        prog = "TextExtractor",
        description="script for extracting text from PDF document")

    argParser.add_argument(
        "-i",
        "--input-path",
        help="path to the PDF document from which text will be extracted",
        required=True)

    argParser.add_argument(
        "-o",
        "--output-path",
        help="path to the file where the text will be saved",
        required=True)

    args = argParser.parse_args()
    extract_text(args.input_path, args.output_path)
