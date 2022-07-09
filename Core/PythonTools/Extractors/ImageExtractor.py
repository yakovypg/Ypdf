# pip install --upgrade pip
# pip install --upgrade pymupdf
# pip install tqdm

import os
import fitz
import argparse

from tqdm import tqdm

def extractImages(destDir, pdfPaths):
    for path in pdfPaths:
        pdfDoc = fitz.Document(path)

        for i in tqdm(range(len(pdfDoc)), desc="pages"):
            for pageImg in tqdm(pdfDoc.get_page_images(i), desc="page_images"):
                extractedImgRef = pageImg[0]
                pdfDoc.extract_image(extractedImgRef)
                pixmap = fitz.Pixmap(pdfDoc, extractedImgRef)
                
                extractedImgName = "%s_p%s_%s.png" % (path[:-4], i, extractedImgRef)
                extractedImgPath = os.path.join(destDir, extractedImgName)
                pixmap.save(extractedImgPath)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="ImageExtractor: script for extracting images from PDF document")
    
    parser.add_argument("-i", "--input-documents", nargs='+', help="PDF documents from which images will be extracted", required=True)
    parser.add_argument("-o", "--output-directory", help="Directory for saving extracted images", default="")

    args = parser.parse_args()

    extractImages(args.output_directory, args.input_documents)