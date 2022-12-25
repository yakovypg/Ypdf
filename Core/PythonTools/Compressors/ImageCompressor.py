# pip install Pillow

from msilib.schema import Extension
import os
import sys
import argparse

from PIL import Image

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

def getFormattedSize(bytesValue, factor=1024, suffix="B"):
    for prefix in ["", "K", "M", "G", "T", "P", "E", "Z"]:
        if bytesValue < factor:
            return f"{bytesValue:.2f} {prefix}{suffix}"
        bytesValue /= factor
       
    return f"{bytesValue:.2f}Y{suffix}"

def compressImage(imagePath, destPath, qualityFactor=0.75, sizeFactor=1.0, width=None, height=None):
    img = None

    try:
        img = Image.open(imagePath)
    except:
        printError()
        return
    
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
        try:
            img = img.convert("RGB")
            img.save(destPath, quality=quality, optimize=True)
        except:
            printError()
            return
    except:
        printError()
        return

    destImgSizeStr = "%sx%s" % (destImgSize[0], destImgSize[1]);

    destImgWeight = os.path.getsize(destPath)
    destImgWeightStr = getFormattedSize(destImgWeight)
    
    compression = (1 - destImgWeight / sourceImgWeight) * 100
    compressionStr = f"{compression:.2f}%"

    _, imgPathTail = os.path.split(imagePath)
    sourceImgName = imgPathTail
    
    print("")
    print("image: %s" % sourceImgName)
    print("size: %s -> %s" % (sourceImgSizeStr, destImgSizeStr))
    print("weight: %s -> %s" % (sourceImgWeightStr, destImgWeightStr))
    print("compression: %s" % compressionStr)

def compressImages(imagePaths, destDir, qualityFactor=0.75, sizeFactor=1.0, extension=None):
    if (extension is not None) and len(extension) == 0:
        print("incorrect file extesion")
        return

    for imagePath in imagePaths:
        imgName = os.path.basename(imagePath)
        imgName = os.path.splitext(imgName)[0] or os.path.splitext(imgName)[1]
        imgExt = os.path.splitext(imagePath)[1] or ".jpg"

        if extension:
            imgExt = extension if extension[0] == "." else ("." + extension)
        
        uniquePath = os.path.join(destDir, "%s_compressed%s" % (imgName, imgExt))
        uniquePath = makePathUnique(uniquePath)

        compressImage(imagePath, uniquePath, qualityFactor, sizeFactor)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="ImageCompressor: script for compressing images")
    
    parser.add_argument("-i", "--input-images", nargs="+", help="Images to compress", required=True)
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
