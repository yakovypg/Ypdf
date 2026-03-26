# pip install PyMuPDF==1.27.2.2 tqdm==4.67.3

import os
import sys
import fitz
import argparse

from tqdm import tqdm

def print_error() -> None:
    exceprion_info = sys.exc_info()
    message = exceprion_info[1]
    print(message)

def make_path_unique(path: str) -> str:
    name, extension = os.path.splitext(path)
    unique_path = path
    counter = 1

    while os.path.isfile(unique_path):
        unique_path = "%s (%s)%s" % (name, counter, extension)
        counter += 1

    return unique_path

def extract_images(
        output_directory: str,
        pdf_paths: str,
        imgs_count_limit: int = -1) -> None:
    for path in pdf_paths:
        try:
            file_basename = os.path.basename(path)
            file_name = os.path.splitext(file_basename)[0] or os.path.splitext(file_basename)[1]

            img_num = 1
            pdf_document = fitz.Document(path)

            for page_num in tqdm(range(len(pdf_document)), desc="pages"):
                for page_img in tqdm(pdf_document.get_page_images(page_num), desc="page_images"):
                    img_ref = page_img[0]
                    pdf_document.extract_image(img_ref)
                    pixmap = fitz.Pixmap(pdf_document, img_ref)

                    img_extension = ".png"
                    img_name = "%s_p%s_r%s_n%s%s" % (file_name, page_num, img_ref, img_num, img_extension)
                    img_path = make_path_unique(os.path.join(output_directory, img_name))

                    pixmap.save(img_path)

                    if img_num == imgs_count_limit:
                        break

                    img_num += 1

                if img_num == imgs_count_limit:
                    break
        except:
            print_error()

if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        prog="ImageExtractor",
        description="script for extracting images from PDF document")

    parser.add_argument(
        "-i",
        "--input-documents",
        nargs='+',
        help="pdf documents from which images will be extracted",
        required=True)

    parser.add_argument(
        "-o",
        "--output-directory",
        help="path to the directory for saving extracted images",
        default="")

    parser.add_argument(
        "-l",
        "--limit",
        type=int,
        help="maximum number of images that can be extracted",
        default=0)

    args = parser.parse_args()
    extract_images(args.output_directory, args.input_documents, args.limit)
