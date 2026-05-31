# Tools Documentation
This document provides essential information on using **Ypdf** and all its tools, guiding you through the functionalities and features available for effective PDF management.

## Get Help
You can get a list of parameters and tools with explanations.
```bash
ypdf --help
```

You can also get this information for individual tools (for example, the merge tool).
```bash
ypdf merge --help
```

## Get Version
You can get the current application version.
```bash
ypdf --version
```

## Change Global Config
You can view the global configuration.
```bash
ypdf config --show
```

You can reset the global configuration to its default values.
```bash
ypdf config --reset
```

You can set global configuration values.
```bash
ypdf config --save \
    --python-alias python3 \
    --python-venv /home/user/.ypdf-venv
```

All supported global configuration values you can see in help menu.
```bash
ypdf config --help
```

## Get PDF Document Information
You can get information about a PDF document.
```bash
ypdf info -i /path/to/input.pdf
```

If the PDF document has a lot of pages, you can set maximum number of page-size entries that will be printed (zero is the default and indicates no limitation).
```bash
ypdf info -i /path/to/input.pdf --limit-page-sizes 5
```

## Split PDF Document
You can split a PDF document using custom page ranges.
```bash
ypdf split -i /path/to/input.pdf -o /path/to/output/dir -p 1-3 6 8-10
```

If you don't specify page ranges, a PDF document will be split into parts of a default size.
```bash
ypdf split -i /path/to/input.pdf -o /path/to/output/dir
```

You can also specify the part size as a number of bytes or an expression.
```bash
ypdf split -i /path/to/input.pdf -o /path/to/output/dir --part-size "10*1024*1024"
```

## Merge PDF Documents
You can merge PDF documents.
```bash
ypdf merge -i /path/to/input1.pdf /path/to/input2.pdf -o /path/to/output.pdf
```

On many platforms you can use a file pattern to specify multiple files.
```bash
ypdf merge -i /path/to/files/*.pdf -o /path/to/output.pdf
```

## Compress PDF Document
The compression function doesn't use special PDF-structure optimizations or advanced image-compression methods. It is primarily intended to compress documents that contain uncompressed images (for example, books with high‑quality images). If the document already contains well‑compressed or lightweight images, compression may not reduce the file size.

You can compress a PDF document by recompressing its images. Please note that Python 3 must be installed on your system to execute this tool.
```bash
ypdf compress -i /path/to/input.pdf -o /path/to/output.pdf
```

JPEG is the default format for extracted images, so images with transparency may be altered. To preserve transparency, choose another format (for example, PNG), but note other formats typically give a lower compression ratio.

You can set the image extension, image quality factor, and image size factor.
```bash
ypdf compress -i /path/to/input.pdf -o /path/to/output.pdf -e png -q 0.5 -s 0.8
```

By default, a validity check verifies that recompressing images will actually reduce their size. If not, the new document will not be created. However, you can disable this check.
```bash
ypdf compress -i /path/to/input.pdf -o /path/to/output.pdf --disable-compression-validity-check
```

You can check whether a PDF document is compressible.
```bash
ypdf can-compress -i /path/to/input.pdf
```

## Copy PDF Document
You can copy a PDF document.
```bash
ypdf copy -i /path/to/input.pdf -o /path/to/output.pdf
```

## Remove Pages From PDF Document
You can remove pages from a PDF document.
```bash
ypdf remove-pages -i /path/to/input.pdf -o /path/to/output.pdf -p 1-3 5 7-10
```

## Move PDF Document Page
You can move a PDF document page.
```bash
ypdf move-page -i /path/to/input.pdf -o /path/to/output.pdf --from 5 --to 10
```

## Reorder PDF Document Pages
You can reorder pages in a PDF document. For example, the following command rearranges pages into the order 5, 3, 2, 1, 4.
```bash
ypdf reorder-pages -i /path/to/input.pdf -o /path/to/output.pdf --page-order 5,3-1,4
```

## Rotate PDF Document Pages
You can rotate all pages of a PDF document by a specified angle. Rotation is limited to multiples of 90 degrees.
```bash
ypdf rotate -i /path/to/input.pdf -o /path/to/output.pdf -a 90
```

You can also rotate specific pages of a PDF document to a specific angle. For example, the following command rotates second page by 90 degrees, and pages 1, 3, 4, 5 by 180 degrees.

```bash
ypdf rotate -i /path/to/input.pdf -o /path/to/output.pdf -r 2:90 1,3-5:180
```

## Crop PDF Document Pages
You can crop specific pages of a PDF to a given rectangle. For example, the following command crops pages 1 and 3 to the rectangle with lower-left corner (30,40) and upper-right corner (60,60).
```bash
ypdf crop -i /path/to/input.pdf -o /path/to/output.pdf --cropping 1,3:(30;40),(60;60)
```

## Divide PDF Document Pages
You can split specific pages of a PDF into two parts. For example, the following command splits page 1 into two equal horizontal parts, and splits pages 3 and 4 vertically with the dividing line shifted 10 pixels to the right (making the right part 10 pixels narrower than the left).
```bash
ypdf divide -i /path/to/input.pdf -o /path/to/output.pdf --division 1:horizontal 3-4:vertical,10
```

## Add Page Numbers To PDF Document
You can add page numbers to a PDF document.
```bash
ypdf add-page-numbers -i /path/to/input.pdf -o /path/to/output.pdf
```

You can also specify horizontal and vertical page number alignment, text presenter, page number margins, and whether to respect the page's existing left, top, right, and bottom margins.
```bash
ypdf add-page-numbers -i /path/to/input.pdf -o /path/to/output.pdf \
    --horizontal-alignment left \
    --vertical-alignment top \
    --num-presenter verbal \
    --margin 0,10 \
    --left-page-margin \
    --top-page-margin \
    --right-page-margin \
    --bottom-page-margin
```

You can configure font size, family, color, and opacity.
```bash
ypdf add-page-numbers -i /path/to/input.pdf -o /path/to/output.pdf \
    --font-size 24 \
    --font-family Times-Bold \
    --font-color cyan \
    --font-opacity 0.5
```

You can also use your own font instead of font family.
```bash
ypdf add-page-numbers -i /path/to/input.pdf -o /path/to/output.pdf \
    --font-size 24 \
    --font-path /path/to/font.ttf \
    --font-encoding Identity-H \
    --font-color cyan \
    --font-opacity 0.5
```

You can increase page size and place page numbers in the added area. To do this, you need to specify the page increase mode. You can also specify the color that will fill the area after increasing the page size and page size adjustment for all pages.
```bash
ypdf add-page-numbers -i /path/to/input.pdf -o /path/to/output.pdf \
    --increase-page-mode bottom \
    --fill-color green \
    --page-size-adjustment 40
```

If increasing the page causes an unexpected ~50‑pixel rightward shift of the first or all page numbers, remove it by specifying a content shift.
```bash
ypdf add-page-numbers -i /path/to/input.pdf -o /path/to/output.pdf --content-shift 1-10:-50,0
```

## Add Watermark To PDF Document
You can add a watermark to a PDF document. Specify pages to process. If no pages are specified, the watermark will be placed on all pages of the PDF document.
```bash
ypdf add-watermark -i /path/to/input.pdf -o /path/to/output.pdf --text "My watermark" -p 1 5-10
```

The watermark is placed inside an enclosing rectangle that rotates with the text. If the text doesn't fit the rectangle, the watermark will not be added. By default the tool attempts to center the watermark by shifting the lower-left corner, but centering can be disrupted when the rectangle is rotated or when a non‑minimal enclosing rectangle is used. You can always set the lower-left corner position manually.

Note that it is important to specify the watermark text width and height to draw and align it correctly.

You can set text, rotation angle, width, height, lower-left position, text alignment, horizontal text alignment, and the watermark container's vertical alignment.
```bash
ypdf add-watermark -i /path/to/input.pdf -o /path/to/output.pdf \
    --text "My watermark" \
    --angle 60 \
    --width 500 \
    --height 100 \
    --position (250;250) \
    --text-alignment left \
    --text-h-alignment left \
    --container-v-alignment bottom
```

You can configure font size, family, color, and opacity.
```bash
ypdf add-watermark -i /path/to/input.pdf -o /path/to/output.pdf \
    --text "My watermark" \
    --font-size 28 \
    --font-family Times-Bold \
    --font-color black \
    --font-opacity 0.5 \
```

You can also use your own font instead of font family.
```bash
ypdf add-watermark -i /path/to/input.pdf -o /path/to/output.pdf \
    --text "My watermark" \
    --font-size 28 \
    --font-path /path/to/font.ttf \
    --font-encoding Identity-H \
    --font-color black \
    --font-opacity 0.5 \
```

You can add a border for the watermark and specify its style, color, thickness, and opacity.
```bash
ypdf add-watermark -i /path/to/input.pdf -o /path/to/output.pdf \
    --text "My watermark" \
    --border dashed \
    --border-color blue \
    --border-thickness 5 \
    --border-opacity 0.7
```

If you don't know the exact text dimensions, you can provide the full area for the watermark. In that case place the text at the area's lower-left corner.
```bash
ypdf add-watermark -i /path/to/input.pdf -o /path/to/output.pdf \
    -t "My watermark" \
    -a 60 \
    --position (250;0) \
    --width 10000 \
    --height 10000 \
    --text-alignment left \
    --text-h-alignment left \
    --container-v-alignment bottom
```

You can also add a watermark annotation. Its configuration is similar to the watermark but doesn't support text alignment, horizontal text alignment, or container vertical alignment. Instead you can specify X-translation and Y-translation.

Unlike the watermark, a watermark annotation is placed in an enclosing rectangle that doesn't rotate with the text. If the text does not fit, it will be clipped. By default the annotation is centered by shifting the lower-left corner. Because the rectangle does not rotate, the annotation stays centered when the text rotates. Centering can still be broken with a non‑minimal enclosing rectangle, and you can always set the lower-left corner manually.

Note that it is important to specify the width, height, X-translation and Y-translation to ensure correct drawing and alignment.
```bash
ypdf add-watermark-annotation -i /path/to/input.pdf -o /path/to/output.pdf \
    -t "My watermark" \
    -a 60 \
    --width 300 \
    --height 450 \
    --x-translation 50 \
    --y-translation 25 \
    --font-size 72 \
    --font-family Times-Bold \
    --font-color black \
    --font-opacity 0.5
```

## Remove Watermark From PDF Document
You can remove a watermark that is stored as a PDF annotation. This doesn't remove watermarks embedded in images or in other non-annotation formats. Specify pages to process. If no pages are specified, the watermark will be removed from all pages.
```bash
ypdf remove-watermark-annotation -i /path/to/input.pdf -o /path/to/output.pdf -p 1 5-10
```

## Convert Images To PDF Document
You can convert one or more images to a single PDF document.
```bash
ypdf image-to-pdf -i /path/to/input1.jpg /path/to/input2.png -o /path/to/output.pdf
```

On many platforms you can use a file pattern to specify multiple files.
```bash
ypdf image-to-pdf -i /path/to/images/*.jpg -o /path/to/output.pdf
```

You can set the PDF page size, image margins, image rotation angle, and horizontal image alignment.
```bash
ypdf image-to-pdf -i /path/to/input1.jpg /path/to/input2.png -o /path/to/output.pdf \
    --page-size a4 \
    --margin 10 \
    --image-h-alignment left \
    --angle 90
```

By default, page sizes will automatically increase to match image dimensions. Note that this automatic page resizing doesn't account for image rotation. You can also disable automatic resizing.
```bash
ypdf image-to-pdf -i /path/to/input1.jpg /path/to/input2.png -o /path/to/output.pdf --disable-autoincrease-size
```

## Convert Text To PDF Document
You can convert text files to a PDF document.
```bash
ypdf text-to-pdf -i /path/to/input.txt -o /path/to/output.pdf
```

You can specify margins, text alignment, and PDF document pages size.
```bash
ypdf text-to-pdf -i /path/to/input.txt -o /path/to/output.pdf -m 50,100,50,100 -a right -s a4
```

You can configure font size, family, color, and opacity.
```bash
ypdf text-to-pdf -i /path/to/input.txt -o /path/to/output.pdf \
    --font-size 28 \
    --font-family Times-Bold \
    --font-color black \
    --font-opacity 0.5 \
```

You can also use your own font instead of font family.
```bash
ypdf text-to-pdf -i /path/to/input.txt -o /path/to/output.pdf \
    --font-size 28 \
    --font-path /path/to/font.ttf \
    --font-encoding Identity-H \
    --font-color black \
    --font-opacity 0.5 \
```

## Extract Images From PDF Document
You can extract images from a PDF document. Please note that Python 3 must be installed on your system to execute this tool.
```bash
ypdf extract-images -i /path/to/input.pdf -o /path/to/output/directory
```

## Extract Text From PDF Document
You can extract text from a PDF document.
```bash
ypdf extract-text -i /path/to/input.pdf -o /path/to/output.txt
```

You can also use Tika for text extraction. In this case Python 3 must be installed on your system.
```bash
ypdf extract-text -i /path/to/input.pdf -o /path/to/output.txt --use-tika
```

## Set Password To PDF Document
You can set password to a PDF document.
```bash
ypdf set-password -i /path/to/input.pdf -o /path/to/output.pdf --password "samePassword"
```

To set different user and owner passwords, provide separate values for each.
```bash
ypdf set-password -i /path/to/input.pdf -o /path/to/output.pdf --user-password "user" --owner-password "admin"
```

You can also specify the encryption algorithm.
```bash
ypdf set-password -i /path/to/input.pdf -o /path/to/output.pdf --password "samePassword" -e encryption_aes_128
```

## Remove Password From PDF Document
You can remove password from a PDF document.
```bash
ypdf remove-password -i /path/to/input.pdf -o /path/to/output.pdf --password "samePassword"
```

To try different user and owner passwords, provide separate values for each.
```bash
ypdf remove-password -i /path/to/input.pdf -o /path/to/output.pdf --user-password "user" --owner-password "admin"
```

## Compress Images
You can compress images. Please note that Python 3 must be installed on your system to execute this tool.
```bash
ypdf compress-images -i /path/to/input.png -o /path/to/output.jpg
```

On many platforms you can use a file pattern to specify multiple files.
```bash
ypdf compress-images -i /path/to/files/*.jpg -o /path/to/output/directory
```

If only one input image is specified, the output format is inferred from the output file path. When processing multiple images, specify an output directory and set the output extension explicitly (JPEG is the default). To achieve a good compression ratio, it is recommended to specify the JPEG format.

You can specify the image quality factor, image size factor (or output image size), and output image extension (in case you specified multiple images).

For example, with this command, you will compress the image with a quality factor of 0.75 and reduce its size by 30%.
```bash
ypdf compress-images -i /path/to/input.png -o /path/to/output.jpg -q 0.75 -s 0.7
```

In another case, with this command, you will compress all images with a quality factor of 0.75, set their size to 1920x1080, and use the JPEG format as the extension for the compressed images.
```bash
ypdf compress-images -i /path/to/files/*.jpg -o /path/to/output/directory \
    --quality-factor 0.75 \
    --width 1920 \
    --height 1080 \
    --extension jpg
```
