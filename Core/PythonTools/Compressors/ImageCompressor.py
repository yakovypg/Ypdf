# pip install Pillow

import os
import argparse

from PIL import Image

def getFormattedSize(bytesValue, factor=1024, suffix="B"):
    for prefix in ["", "K", "M", "G", "T", "P", "E", "Z"]:
        if bytesValue < factor:
            return f"{bytesValue:.2f} {prefix}{suffix}"
        bytesValue /= factor
       
    return f"{bytesValue:.2f}Y{suffix}"

def compressImage(imagePath, destPath, qualityFactor=0.75, sizeFactor=1.0, width=None, height=None):
    img = Image.open(imagePath)
    
    sourceImgSize = img.size
    sourceImgSizeStr = "%sx%s" % (sourceImgSize[0], sourceImgSize[1]);
    
    sourceImgWeight = os.path.getsize(imagePath)
    sourceImgWeightStr = getFormattedSize(sourceImgWeight)
    
    destImgSize = img.size
    quality = int(qualityFactor * 100)

    if width and height:
        img = img.resize((width, height), Image.Resampling.LANCZOS)
        destImgSize = img.size
        
    elif sizeFactor != 1.0:
        newWidth = int(img.size[0] * sizeFactor)
        newHeight = int(img.size[1] * sizeFactor)
        newSize = (newWidth, newHeight)
        
        img = img.resize(newSize, Image.Resampling.LANCZOS)
        destImgSize = img.size
        
    try:
        img.save(destPath, quality=quality, optimize=True)
    except OSError:
        img = img.convert("RGB")
        img.save(destPath, quality=quality, optimize=True)
    
    destImgSizeStr = "%sx%s" % (destImgSize[0], destImgSize[1]);

    destImgWeight = os.path.getsize(destPath)
    destImgWeightStr = getFormattedSize(destImgWeight)
    
    compression = (1 - destImgWeight / sourceImgWeight) * 100
    compressionStr = f"{compression:.2f}%"
    
    imgPathHeadTail = os.path.split(imagePath)
    sourceImgName = imgPathHeadTail[1]
    
    print("")
    print("image: %s" % sourceImgName)
    print("size: %s -> %s" % (sourceImgSizeStr, destImgSizeStr))
    print("weight: %s -> %s" % (sourceImgWeightStr, destImgWeightStr))
    print("compression: %s" % compressionStr)

def compressImages(imagePaths, destDir, qualityFactor=0.75, sizeFactor=1.0, extension=None):
    for imagePath in imagePaths:
        imgName, imgExt = os.path.splitext(imagePath)
        
        if extension:
            imgExt = extension
        
        destPath = os.path.join(destDir, "%s_compressed.%s" % (imgName, imgExt))       
        compressImage(imagePath, destPath, qualityFactor, sizeFactor)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="ImageCompressor: script for compressing images")
    
    parser.add_argument("-i", "--input-images", nargs='+', help="Images to compress", required=True)
    parser.add_argument("-o", "--output-path", help="Compressed image path", default=None)
    parser.add_argument("-O", "--output-directory", help="Directory for saving compressed images", default="")
    parser.add_argument("-e", "--extension", help="Extension in which the images will be converted", default=None)
    parser.add_argument("-q", "--quality", type=float, help="Output image quality [from 0.0 (worst) to 0.95 (best)]. Default is 0.75", default=0.75)
    parser.add_argument("-s", "--size-factor", type=float, help="Resizing factor (setting to X will multiply width & height by X). Default is 1.0", default=1.0)
    parser.add_argument("-W", "--width", type=int, help="New width of the image. Make sure to set it with the `height` parameter", default=None)
    parser.add_argument("-H", "--height", type=int, help="New height of the image. Make sure to set it with the `width` parameter", default=None)
    
    args = parser.parse_args()
        
    if args.output_path:
        compressImage(args.input_images[0], args.output_path, args.quality, args.size_factor, args.width, args.height)
    else:
        compressImages(args.input_images, args.output_directory, args.quality, args.size_factor, args.extension)