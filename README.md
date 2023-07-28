# Ypdf
Ypdf is a cross-platform tool for converting and performing other actions on PDF documents. 
The tool has both a CLI and a graphical version, so that it is convenient for both 
command-line lovers and people who prefer graphical shells.

Graphic version is currently under development.

## Start of use

### Tool features

With this tool you will be able to perform the following actions:
- Split PDF document
- Merge PDF documents
- Compress PDF document
- Copy PDF document
- Remove pages from PDF document
- Move PDF document page
- Reorder PDF document pages
- Rotate PDF document pages
- Crop PDF document pages
- Divide PDF document pages
- Add page numbers to PDF document
- Add watermark to PDF document
- Remove watermark from PDF document
- Convert images to PDF document
- Convert text to PDF document
- Extract images from PDF document
- Extract text from PDF document
- Set password to PDF document
- Remove password from PDF document
- Compress images

### Release usage

To use the built project, follow these steps:
1. Choose the release.
2. Download and unzip the archive with the version for your operating system and architecture.
3. Move the folder to any place convenient for you. The tool does not create any files outside 
of its directory.
4. Go to this folder and run the executable file.

You can also create a shortcut to the executable file and put it in a convenient place for you.

Please note that if you want to run the CLI version of the tool, run the executable file from 
the console. For example, you can find out the version of the tool using the on of the command
below.

On Windows.

```
ypdf --version
```

On Linux and Mac.

```
./ypdf --version
```

### Build from source

We will build the project using [dotnet](https://dotnet.microsoft.com/en-us/). But you can 
also use the IDE tools, then the project will be built in one click of the corresponding 
button.

First, clone the repository.

```
git clone https://github.com/yakovypg/Ypdf.git
```

Go to the project folder.

```
cd Ypdf
```

[Build](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build) the project.

```
dotnet build -c Release
```

Now you can already run, for example, the CLI version of the tool.

```
dotnet run --project Applications/ypdf
```

You can also [publish](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish) 
the project. For example, we will publish the CLI version.

```
dotnet publish Applications/ypdf -c Release -o pathToPublishFolder
```

The published project is runtime-dependent. That is, to run it, it is necessary that .NET 
runtime is installed on your system. You can run the tool using the following command:

```
dotnet pathToPublishFolder/ypdf.dll
```

You can also publish the project with the --self-contained parameter. Then, to run the tool, 
it is not required that .NET runtime is installed on your system. To publish a project with 
this parameter, you should also specify the target platform and architecture using -r 
parameter. For example, we will publish a project for linux-x64.

```
dotnet publish Applications/ypdf -c Release -o pathToPublishFolder --self-contained True -r linux-x64
```

A published project can be run without dotnet. To do this, navigate to the folder where you 
published the project and enter one of the commands below.

On Windows.

```
ypdf --version
```

On Linux and Mac.

```
./ypdf --version
```

### Development

The project is developed for the .NET 6.0 platform. To continue development, you will need the 
.NET SDK and .NET Runtime.

## Basics

- PDF document page numbers start at 1.
- To perform some actions on a PDF document, it is necessary that it be without a password.
- To perform some actions on a PDF document, it is necessary that 
[python 3](https://www.python.org/downloads/) and some of its 
[packages](#python-packages-installation) are installed.
- To perform some actions on a PDF document, it is necessary that 
[java 8](https://www.java.com/en/download/manual.jsp) is installed.

### Python packages installation

First, upgrade pip.

```
pip install --upgrade pip
```

Next, install all the necessary packages with the following command.

```
pip install -r requirements.txt
```

There is no need to install specific versions of packages. However, the tool has been 
tested against those specified in the corresponding file.

## How to use CLI version

### Basics

#### The most important parameters

- You can specify the path to the input file using the -i parameter.
- You can specify the path to the output file using the -o parameter.
- You can specify the path to the output directory using the -O parameter.
- You can specify the paths to multiple files using the -f parameter multiple times.
- You can specify the paths to directories from which files will be taken using the 
--files-from parameter.
- You can specify the pattern to match files taken from the directories specified by 
the --files-from parameter using the --file-pattern parameter.

#### Font configuration

- You can specify font size, font family, font color and font opacity after the -s, 
--font-family, --font-color and --font-opacity parameters.
- You can also specify your own font. To do this, you need to specify the font path after the 
--font-path parameter (and the encoding after the --font-encoding parameter, if necessary).

#### Parameters parsing

- The *range* can be represented by either one number or two. The numbers are separated by 
the '-' symbol (the first number means the start of the range and the second number means 
the end of the range).
- The *page order* should be represented as a sequence of numbers or ranges separated by 
commas. These items mean the page numbers through which the page order is determined.
- The *rotation* should be represented as two elements separated by the ':' symbol. The first 
element means the set of comma-separated page numbers (that can be represented either as a 
single number or as range) and the second element means the angle to which this pages should 
be rotated.
- The *cropping* should be represented as two elements separated by the ':' symbol. The first
element means the set of comma-separated page numbers (that can be represented either as a 
single number or as range) and the second element means the pair of points separated by comma, 
which corresponds to the lower-left and upper-right corners of the rectangle to be cut.
- The *division* should be represented as two elements separated by the ':' symbol. The first
element means the set of comma-separated page numbers (that can be represented either as a 
single number or as range) and the second element means the pair of the orientation of the 
division and the offset of the central line of the division.The shift is a floating-point 
number (you can not explicitly specify it, in this case, write only the orientation, the shift 
will be automatically set to 0). The parameter value is either the string 'vertical' or 
'horizontal'.
- The *content shift* should be represented as two elements separated by the ':' symbol. The 
first element means the set of comma-separated page numbers (that can be represented either 
as a single number or as range) and the second element means the pair of comma-separated 
numbers (the first number means a horizontal shift and the second means a vertical shift).
- The *splitting part* can be represented by either a number or a simple expression that uses 
only multiplication.
- The *page size* can be represented either by a special name or by two numbers separated by 
the 'x' symbol (the first number means the page width, and the second number means the page 
height).
- The *page increase* should be represented as four numbers separated by commas. The first 
number means an increase to the left, the second number means an increase to the top, the 
third number means an increase to the right, the fourth number means an increase to the bottom.
- The *margin* can be represented by either one number (this number will be an indent on all 
sides), two numbers (the first number will be an indent on the left and right, and the second 
number will be an indent on the top and bottom), or four numbers (the first number will be an 
indent on the left, the second number will be an indent on the top, the third number will be 
an indent on the right, the fourth number will be an indent on the bottom).
- The *image extension* should be represented without a dot.
- You can get special names for some parameters using "ypdf --help".
- Floating point numbers are converted depending on your environment (that is, the fractional 
part is separated by a dot or comma depending on your environment).

### Get help

Getting the list of parameters with explanations.

```
ypdf --help
```

### Get version

Getting the current tool version.

```
ypdf --version
```

### Get guide

Getting the guide.

```
ypdf --guide
```

### Global config

You can get the current global configuration using the --show-config parameter.

```
ypdf config --show-config=true
```

You can save the current global configuration using the --save-config parameter.

```
ypdf config --save-config=true
```

You can reset the current global configuration using the --reset-config parameter.
In this case, the configuration will be automatically saved.

```
ypdf config --reset-config=true
```

To set values for global configuration variables, use the parameters used to set local values 
in conjunction with the --save-config parameter.

```
ypdf config --python-alias=python3 --save-config=true
```

The following global variables are available:
- python-alias

### Split PDF document

Splitting the PDF document.

Splitting ranges are specified after the -p parameter. Parts of the PDF document will be placed
in a directory that can be specified after the -O parameter. [Here](#parameters-parsing) you can 
read about the format of the input parameters.

```
ypdf split -i inputPath.pdf -O outputDirectoryPath -p 1-3 -p 6 -p 8-10
```

If you do not specify splitting ranges, the PDF document will be split into parts by default 
size.

```
ypdf split -i inputPath.pdf -O outputDirectoryPath
```

You can also specify the size of these parts using the --spit-part parameter. It can be either 
a number or a simple expression that uses only multiplication.

```
ypdf split -i inputPath.pdf -O outputDirectoryPath --split-part=1024*1024*3
```

### Merge PDF documents

Merging the PDF documents.

```
ypdf merge -f inputPath1.pdf -f inputPath2.pdf -o outputPath.pdf
```

You can also specify the directory from which the files will be taken using the --files-from 
parameter and the pattern that these files should satisfy using the --file-pattern parameter.

```
ypdf merge --files-from=pathToDir1 --files-from=pathToDir2 --file-pattern=*.pdf -o outputPath.pdf
```

### Compress PDF document

Compressing the PDF document by compressing the images contained in it.

To perform this action, it is necessary that python 3 and some of its packages are installed.

```
ypdf compress -i inputPath.pdf -o outputPath.pdf
```

You can specify image quality factor (from 0 to 1), image size factor (from 0 to 1) and images 
extension after the --image-quality, --image-size-factor and -E parameters.

JPEG is the default image format. Therefore, images with transparency can be distorted. To avoid 
this, use a different format (for example, PNG). However, other formats give a much lower 
compression ratio.

```
ypdf compress -i inputPath.pdf -o outputPath.pdf --image-quality=0.75 --image-size-factor=0.5 -E jpg
```

Before compression, by default, a check is performed to ensure that the extracted images 
actually reduce their size with the specified compression parameters. If there is no reduction, 
the document is not compressed and the corresponding message is displayed. To disable this check, 
you need to set the --check-compression-validity parameter to False.

```
ypdf compress -i inputPath.pdf -o outputPath.pdf --check-compression-validity=false
```

You can also check if the document can be compressed using the following command

```
ypdf can-compress -i inputPath.pdf
```

It is worth noting that the compression check function has some error. This is especially true 
when image compression leads to a small reduction in file size.

The compression function is implemented without any special image compression methods or the 
structure of the PDF document as a whole. First of all, this function is designed to compress 
documents containing uncompressed images (for example, to compress a book containing
high-quality images). Due to this implementation, document compression will not reduce the file 
size if the document originally contained sufficiently compressed or lightweight images.

### Copy PDF document

Copying the PDF document.

```
ypdf copy -i inputPath.pdf -o outputPath.pdf
```

### Remove pages from PDF document

Removing the specified pages from the PDF document.

Pages are specified after the -p parameter and can be either single numbers or ranges. 
[Here](#parameters-parsing) you can read about the format of the input parameters.

```
ypdf remove-pages -i inputPath.pdf -o outputPath.pdf -p 1-3 -p 5 -p 7-10
```

### Move PDF document page

Moving the PDF document page.

You should specify the page number to be moved using the -F parameter and the page number to 
which the page should be moved using the -T parameter.

```
ypdf move-page -i inputPath.pdf -o outputPath.pdf -F 5 -T 10
```

### Reorder PDF document pages

Reordering the PDF document pages.

The page order is specified after the --page-order parameter. Make sure that the page order 
contains all the pages of the PDF document. [Here](#parameters-parsing) you can read about 
the format of the input parameters.

For example, with this command you will change the page order from 1,2,3,4,5 to 5,3,2,1,4.

```
ypdf reorder-pages -i inputPath.pdf -o outputPath.pdf --page-order=5,3-1,4
```

### Rotate PDF document pages

Rotating the PDF document pages.

You can rotate all pages of a PDF document by specifying after the -a parameter the angle you
want the pages to be rotated. It is possible to rotate only by an angle that is a multiple of
90 degrees.

```
ypdf rotate -i inputPath.pdf -o outputPath.pdf -a 90
```

You can also rotate specific pages of a PDF document to a specific angle. To do this, you need 
to specify the rotations (the page number and the angle by which you want to rotate the 
specified page) using the -r parameter. [Here](#parameters-parsing) you can read about the 
format of the input parameters.

For example, with this command you will rotate the first and second pages by 90 degrees, and 
the fifth page by 180 degrees.

```
ypdf rotate -i inputPath.pdf -o outputPath.pdf -r 1-2:90 -r 5:180
```

### Crop PDF document pages

Cropping the PDF document pages.

You can crop specific pages of a PDF document by the specific rectangle. To do this, you need 
to specify the croppings (the page number and corners of rectangle by which you want to crop 
the specified page) using the --cropping parameter. [Here](#parameters-parsing) you can read 
about the format of the input parameters.

For example, with this command you will crop the first and third page by rectangle, the 
lower-left corner of which is a point (30;40), and the upper-right corner is a point (60;60).

```
ypdf crop -i inputPath.pdf -o outputPath.pdf --cropping=1,3:(30;40),(60;60)
```

### Divide PDF document pages

Dividing the PDF document pages.

You can divide specific pages of a PDF document into two parts. To do this, you need to specify 
the divisions (the page number, orientation of the division and the offset of the central line 
of the division) using the --division parameter. [Here](#parameters-parsing) you can read 
about the format of the input parameters.

For example, with this command you will divide the first page into two equal parts horizontally 
and the third and fourth pages into two parts vertically with a shift of the dividing line by 
100 pixels to the right (the right part will be 100 pixels smaller than the left).

```
ypdf divide -i inputPath.pdf -o outputPath.pdf --division=1:horizontal --division=3-4:vertical,100
```

### Add page numbers to PDF document

Adding page numbers to the PDF document.

```
ypdf add-page-numbers -i inputPath.pdf -o outputPath.pdf
```

You can horizontal alignment, vertical alignment, margins, considering left page margin, 
considering top page margin, considering right page margin, considering bottom page margin 
and text presenter after the --h-num-alignment, --v-num-alignment, -m, --left-page-margin, 
--top-page-margin, --right-page-margin, --bottom-page-margin and --num-presenter parameters.
[Here](#parameters-parsing) you can read about the format of the input parameters.

```
ypdf add-page-numbers -i inputPath.pdf -o outputPath.pdf --h-num-alignment=left --v-num-alignment=top -m 0,10 --left-page-margin=true --top-page-margin=true --right-page-margin=true --bottom-page-margin=true --num-presenter=verbal
```

You can also [configure the font](#font-configuration).

```
ypdf add-page-numbers -i inputPath.pdf -o outputPath.pdf -s 24 --font-family=times_bold --font-color=blue --font-opacity=0.5
```

You can increase the size of the PDF document pages and put the page numbers in the resulting 
space. To do this, you need to specify the page increase using the --page-increase parameter 
and the location mode using --num-location-mode parameter. You can also specify the color with 
which the resulting space will be filled using the --fill-color parameter.

```
ypdf add-page-numbers -i inputPath.pdf -o outputPath.pdf --fill-color=green --page-increase=0,0,0,40 --num-location-mode=increase_bottom
```

On some PDF documents, when the page is increased on the left, there is sometimes an 
unexpected shift of the first or all page numbers at once by about 50 pixels to the right. To 
remove this shift, specify the content shift that will smooth out the unexpected shift using 
the --content-shift parameter.

```
ypdf add-page-numbers -i inputPath.pdf -o outputPath.pdf --content-shift=1-10:-50,0
```

### Add watermark to PDF document

Adding watermark to the PDF document.

You can specify the pages on which to place the watermark using the -p parameter. If page 
numbers are not specified, the watermark will be placed on all pages of the PDF document.
[Here](#parameters-parsing) you can read about the format of the input parameters.

```
ypdf add-watermark -i inputPath.pdf -o outputPath.pdf -p 1 -p 5-10
```

You can specify text, rotation angle, width, height and lower left point after the -w, -a, 
--watermark-opacity, --watermark-width, --watermark-height and --watermark-pos parameters.

You can also [configure the font](#font-configuration).

The watermark is placed in an enclosing rectangle that rotates along with the text. If the 
text does not fit into this rectangle, the watermark will not be added. By default, an attempt 
is made to center the watermark by shifting the position of the lower-left corner. But 
centering can be broken during turns or when a non-minimal enclosing rectangle is specified. 
But you can always specify the position of the lower-left corner manually using the 
--watermark-pos parameter.

Please note that it is important to specify the width and height of the watermark text in 
order to properly draw and align it.

```
ypdf add-watermark -i inputPath.pdf -o outputPath.pdf -w "My watermark" -s 72 --font-family=times_bold --font-color=black -a 60 --font-opacity=0.5 --watermark-width=500 --watermark-height=100 --watermark-pos=(250;250)
```

You can also add a watermark annotation to the PDF document. The watermark annotation 
configuration is the same as the watermark configuration. But you can also specify 
x-translation and y-translation after the --watermark-x-translation and 
--watermark-y-translation parameters.

Unlike a watermark, the watermark annotation is placed in an enclosing rectangle that does 
not rotate with the text. If the text does not fit into this rectangle, it will be cut off.
As with the watermark, by default there is an attempt to center the watermark annotation by 
shifting the position of the lower left corner. But since the enclosing rectangle does not 
rotate, the watermark annotation remains in the center when the text rotates. However, 
centering may be broken when a non-minimal enclosing rectangle is specified. But you can 
always specify the position of the lower-left corner manually using the --watermark-pos 
parameter.

Please note that it is important to specify the width, height, x-translation and y-translation
of the watermark text in order to properly draw and align it.

```
ypdf add-watermark-annotation -i inputPath.pdf -o outputPath.pdf -w "My watermark" -s 72 --font-family=times_bold --font-color=black -a 60 --font-opacity=0.5 --watermark-width=300 --watermark-height=450 --watermark-x-translation=50 --watermark-y-translation=25
```

### Remove watermark from PDF document

Removing watermark from the PDF document.

It is only possible to remove a watermark represented as a PDF document annotation format. It 
is not possible to remove a watermark from images in a PDF document and watermarks in other 
formats.

```
ypdf remove-watermark-annotation -i inputPath.pdf -o outputPath.pdf
```

You can specify the pages on which to remove the watermark using the -p parameter. If page 
numbers are not specified, the watermark will be removed from all pages of the PDF document.
[Here](#parameters-parsing) you can read about the format of the input parameters.

```
ypdf remove-watermark-annotation -i inputPath.pdf -o outputPath.pdf -p 1 -p 5-10
```

### Convert images to PDF document

Converting the images to the PDF document.

```
ypdf image-to-pdf -f inputPath1.png -f inputPath2.jpg -o outputPath.pdf
```

You can specify PDF document pages size, auto increasing PDF document pages size, images 
margins, images horizontal alignment and images rotation angle after the --page-size, 
--image-autoincrease-size, -m, --image-h-alignment and -a parameters. You can also specify 
the PDF documnet pages size using the --page-width and --page-height parameters. 
[Here](#parameters-parsing) you can read about the format of the input parameters.

Please note that automatic page resizing does not take into account image rotation.

```
ypdf image-to-pdf -f inputPath1.png -f inputPath2.jpg -o outputPath.pdf --page-size=a4 --image-autoincrease-size=false -m 10 --image-h-alignment=left
```

You can also specify the directory from which the files will be taken using the --files-from 
parameter and the pattern that these files should satisfy using the --file-pattern parameter.

```
ypdf image-to-pdf --files-from=pathToDir1 --files-from=pathToDir2 --file-pattern=*.jpg -o outputPath.pdf
```

### Convert text to PDF document

Converting text to the PDF document.

```
ypdf text-to-pdf -i inputPath.txt -o outputPath.pdf
```

You can specify margins, text alignment and PDF document pages size after the -m, -A and 
--page-size parameters. You can also set the page size using the --page-width and 
--page-height parameters. [Here](#parameters-parsing) you can read about the format of the 
input parameters.

```
ypdf text-to-pdf -i inputPath.txt -o outputPath.pdf -m 50,100,50,100 -A right --page-size=a4
```

You can also [configure the font](#font-configuration).

```
ypdf text-to-pdf -i inputPath.txt -o outputPath.pdf -s 24 --font-family=times_bold --font-color=blue --font-opacity=0.5
```

```
ypdf text-to-pdf -i inputPath.txt -o outputPath.pdf --font-path=D://Fonts//Roboto-light.ttf --font-encoding identity_h
```

### Extract images from PDF document

Extracting images from the PDF document.

To perform this action, it is necessary that python 3 and some of its packages are installed.

You can specify the directory where the images will be extracted using the -O parameter and 
one or more PDF documents from which images will be extracted using the -f parameter.

```
ypdf extract-images -f inputPath.pdf -O outputDirectoryPath
```

### Extract text from PDF document

Extracting text from the PDF document.

```
ypdf extract-text -i inputPath.pdf -o outputPath.txt
```

You can also extract the text using the tika. To perform this action, it is necessary that 
java 8, python 3 and some of its packages are installed.

```
ypdf extract-text-tika -i inputPath.pdf -o outputPath.txt
```

### Set password to PDF document

Setting password to the PDF document.

You can specify PDF document password using the --password parameter. In this case, the same 
user password and the owner password will be set. You can also specify the user password and 
the owner password using the --user-password and --owner-password parameters.

```
ypdf set-password -i inputPath.pdf -o outputPath.pdf --password=samePassword
```

```
ypdf set-password -i inputPath.pdf -o outputPath.pdf --user-password=user --owner-password=admin
```

You can also specify the encryption algorithm using the --encryption-algorithm parameter.

```
ypdf set-password -i inputPath.pdf -o outputPath.pdf --user-password=user --owner-password=admin --encryption-algorithm=aes_128
```

### Remove password from PDF document

Removing password from the PDF document.

You should specify the PDF document password using one of the following parameters: 
--password, --user-password, --owner-password.

```
ypdf remove-password -i inputPath.pdf -o outputPath.pdf --password=12345
```

### Compress images

Compressing the images.

To perform this action, it is necessary that python 3 and some of its packages are installed.

You can specify any number of images by using the -f parameter.

If you specified only one image, the output image format is read from the output file path, 
which should be specified after the -o parameter. For a good compression ratio, it is 
recommended to specify the jpg format.

```
ypdf compress-images -f inputPath.png -o outputPath.jpg
```

You can specify image quality factor (from 0 to 1), image size factor (from 0 to 1), output 
image width and output image height after the --image-quality, --image-size-factor, 
--image-width and --image-height parameters.

For example, with this command you will compress the image with an quality factor of 0.75 
and reduce its size by 30%.

```
ypdf compress-images -f inputPath.png -o outputPath.jpg -Q 0.75 --image-size-factor=0.7
```

For example, with this command you will compress the image with a compression ratio of 0.75 
and make its size equal to 1920x1080.

```
ypdf compress-images -f inputPath.png -o outputPath.jpg -Q 0.75 --image-width=1920 --image-height=1080
```

If you have specified several images, you do not need to specify the output path, but you 
can specify the output directory path using the -O parameter.

```
ypdf compress-images -f inputPath1.jpg -f inputPath2.png -O outputDirectoryPath
```

You can also specify image quality factor (from 0 to 1), image size factor (from 0 to 1) 
and output images extension after the --image-quality, --image-size-factor and -E parameters.

JPEG is the default image format. Therefore, images with transparency can be distorted. To avoid 
this, use a different format (for example, PNG). However, other formats give a much lower 
compression ratio.

```
ypdf compress-images -f inputPath1.jpg -f inputPath2.png -O outputDirectoryPath -Q 0.75 --image-size-factor=0.7 -E jpg
```

You can also specify the directory from which the files will be taken using the --files-from 
parameter and the pattern that these files should satisfy using the --file-pattern parameter.

```
ypdf compress-images --files-from=pathToDir1 --files-from=pathToDir2 --file-pattern=*.png -O outputDirectoryPath
```
