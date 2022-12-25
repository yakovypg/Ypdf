# pip install tika

import sys
import argparse

from tika import parser

def printError():
    exceprionInfo = sys.exc_info()
    message = exceprionInfo[1] 
    print(message)

def extractText(inputFile, destPath):
    try:
        doc = parser.from_file(inputFile)
        content = doc['content']
    
        file = open(destPath, 'w', encoding='utf-8')
        file.write(content)
    except:
        printError()

if __name__ == "__main__":
    argParser = argparse.ArgumentParser(description="TextExtractor: script for extracting text from PDF document")
    
    argParser.add_argument("-i", "--input-path", help="Path to the PDF document from which text will be extracted", required=True)
    argParser.add_argument("-o", "--output-path", help="Path to the file where the text will be saved", required=True)

    args = argParser.parse_args()

    extractText(args.input_path, args.output_path)
