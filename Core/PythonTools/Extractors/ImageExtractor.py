# pip install --upgrade pip
# pip install --upgrade pymupdf
# pip install tqdm

import os
import sys
import fitz
import argparse

from tqdm import tqdm

def printError():
    exceprionInfo = sys.exc_info()
    message = exceprionInfo[1] 
    print(message)

def makePathUnique(path):
    name, extension = os.path.splitext(path)
    uniquePath = path    
    counter = 1

    while os.path.isfile(uniquePath):
        uniquePath = "%s (%s)%s" % (name, counter, extension)
        counter = counter + 1

    return uniquePath

def extractImages(destDir, pdfPaths, imgsCountLimit = -1):
    for path in pdfPaths:
        try:
            fileBasename = os.path.basename(path)
            fileName = os.path.splitext(fileBasename)[0] or os.path.splitext(fileBasename)[1]
            
            imgNum = 0
            pdfDoc = fitz.Document(path)

            for pageNum in tqdm(range(len(pdfDoc)), desc="pages"):
                for pageImg in tqdm(pdfDoc.get_page_images(pageNum), desc="page_images"):
                    imgRef = pageImg[0]
                    pdfDoc.extract_image(imgRef)
                    pixmap = fitz.Pixmap(pdfDoc, imgRef)
                
                    imgExt = ".png"
                    imgName = "%s_p%s_r%s_n%s%s" % (fileName, pageNum, imgRef, imgNum, imgExt)
                    imgPath = makePathUnique(os.path.join(destDir, imgName))

                    pixmap.save(imgPath)

                    if imgNum == imgsCountLimit:
                        break

                    imgNum = imgNum + 1

                if imgNum == imgsCountLimit:
                    break
        except:
            printError()

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="ImageExtractor: script for extracting images from PDF document")
    
    parser.add_argument("-i", "--input-documents", nargs='+', help="PDF documents from which images will be extracted", required=True)
    parser.add_argument("-o", "--output-directory", help="Directory for saving extracted images", default="")
    parser.add_argument("-l", "--limit", type=int, help="Maximum number of images that can be extracted", default=0)

    args = parser.parse_args()

    extractImages(args.output_directory, args.input_documents, args.limit)
