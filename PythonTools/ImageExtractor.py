# pip install --upgrade pip
# pip install --upgrade pymupdf
# pip install tqdm

import sys
import os
import fitz

from tqdm import tqdm

def extractImages(destDir, paths):
    for path in paths:
        pdfDoc = fitz.Document(path)

        for i in tqdm(range(len(pdfDoc)), desc="pages"):
            for pageImg in tqdm(pdfDoc.get_page_images(i), desc="page_images"):
                extractedImgRef = pageImg[0]
                pdfDoc.extract_image(extractedImgRef)
                pixmap = fitz.Pixmap(pdfDoc, extractedImgRef)
                pixmap.save(os.path.join(destDir, "%s_p%s_%s.png" % (path[:-4], i, extractedImgRef)))

args = sys.argv
paths = args[2:]
destDir = args[1]

extractImages(destDir, paths)