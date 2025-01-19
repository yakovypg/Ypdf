# pip install Pillow

import os
import sys
import argparse

from PIL import Image

def print_error() -> None:
    exception_info = sys.exc_info()
    message = exception_info[1] 
    print(message)

def make_path_unique(path: str) -> str:
    name, extension = os.path.splitext(path)
    unique_path = path    
    counter = 1

    while os.path.isfile(unique_path):
        unique_path = "%s (%s)%s" % (name, counter, extension)
        counter += 1

    return unique_path

def get_formatted_size(bytes_value: float, factor: int = 1024, suffix: str = "B") -> str:
    for prefix in ["", "K", "M", "G", "T", "P", "E", "Z"]:
        if bytes_value < factor:
            return f"{bytes_value:.2f} {prefix}{suffix}"
        bytes_value /= factor
       
    return f"{bytes_value:.2f}Y{suffix}"

def compress_image(
        image_path: str,
        output_path: str,
        quality_factor: float = 0.75,
        size_factor: float = 1.0,
        new_width: int = None,
        new_height: int = None) -> None:
    img = None

    try:
        img = Image.open(image_path)
    except:
        print_error()
        return
    
    source_img_size = img.size
    source_img_size_str = "%sx%s" % (source_img_size[0], source_img_size[1])
    
    source_img_weight = os.path.getsize(image_path)
    source_img_weight_str = get_formatted_size(source_img_weight)
    
    output_img_size = img.size
    quality = int(quality_factor * 100)

    if new_width or new_height:
        new_width = new_width if new_width else img.size[0]
        new_height = new_height if new_height else img.size[1]
        img = img.resize((new_width, new_height), Image.Resampling.LANCZOS)
        output_img_size = img.size
    elif size_factor != 1.0:
        new_width = int(img.size[0] * size_factor)
        new_height = int(img.size[1] * size_factor)
        new_size = (new_width, new_height)
        
        img = img.resize(new_size, Image.Resampling.LANCZOS)
        output_img_size = img.size
        
    try:
        img.save(output_path, quality=quality, optimize=True)
    except OSError:
        try:
            img = img.convert("RGB")
            img.save(output_path, quality=quality, optimize=True)
        except:
            print_error()
            return
    except:
        print_error()
        return

    output_img_size_str = "%sx%s" % (output_img_size[0], output_img_size[1])

    output_img_weight = os.path.getsize(output_path)
    output_img_weight_str = get_formatted_size(output_img_weight)
    
    compression = (1 - output_img_weight / source_img_weight) * 100
    compression_str = f"{compression:.2f}%"

    _, img_path_tail = os.path.split(image_path)
    source_img_name = img_path_tail
    
    print("")
    print("image: %s" % source_img_name)
    print("size: %s -> %s" % (source_img_size_str, output_img_size_str))
    print("weight: %s -> %s" % (source_img_weight_str, output_img_weight_str))
    print("compression: %s" % compression_str)

def compress_images(
        image_paths: list[str],
        output_directory: str,
        quality_factor: float = 0.75,
        size_factor: float = 1.0,
        extension: str = None) -> None:
    if (extension is not None) and len(extension) == 0:
        print("incorrect file extesion")
        return

    for image_path in image_paths:
        img_name = os.path.basename(image_path)
        img_name = os.path.splitext(img_name)[0] or os.path.splitext(img_name)[1]
        img_extension = os.path.splitext(image_path)[1] or ".jpg"

        if extension:
            img_extension = extension if extension[0] == "." else ("." + extension)
        
        unique_path = os.path.join(output_directory, "%s_compressed%s" % (img_name, img_extension))
        unique_path = make_path_unique(unique_path)

        compress_image(image_path, unique_path, quality_factor, size_factor)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        prog = "ImageCompressor",
        description="script for compressing images")
    
    parser.add_argument(
        "-i",
        "--input-images",
        nargs="+",
        help="images to compress",
        required=True)

    parser.add_argument(
        "-o",
        "--output-path",
        help=("path to the compressed image or path to the directory for saving compressed " +
             "images if several input images specified"),
        default=None)
    
    parser.add_argument(
        "-e",
        "--extension",
        help="extension in which the images will be converted",
        default=None)
    
    parser.add_argument(
        "-q",
        "--quality",
        type=float,
        help="output image quality [from 0.0 (worst) to 0.95 (best)]. Default is 0.75",
        default=0.75)
    
    parser.add_argument(
        "-s",
        "--size-factor",
        type=float,
        help="resizing factor (setting to X will multiply width & height by X). Default is 1.0",
        default=1.0)
    
    parser.add_argument(
        "-W",
        "--width",
        type=int,
        help="new width of the image",
        default=None)
    
    parser.add_argument(
        "-H",
        "--height",
        type=int,
        help="new height of the image",
        default=None)
    
    args = parser.parse_args()
        
    if len(args.input_images) == 1:
        compress_image(
            args.input_images[0],
            args.output_path,
            args.quality,
            args.size_factor,
            args.width,
            args.height)
    else:
        compress_images(
            args.input_images,
            args.output_path,
            args.quality,
            args.size_factor,
            args.extension)
