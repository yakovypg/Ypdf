# pip install tika

import sys
from tika import parser

def extractText(destPath, inputPath):
    doc = parser.from_file(inputPath)
    content = doc['content']
    
    file = open(destPath, 'w', encoding='utf-8')
    file.write(content)

args = sys.argv
destPath = args[1]
inputPath = args[2]

extractText(destPath, inputPath)