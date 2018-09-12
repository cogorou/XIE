#

vpath %.cpp $(SRCDIR)/$(PREFIX)
vpath %.cpp $(SRCDIR)/$(PREFIX)/File
vpath %.cpp $(SRCDIR)/$(PREFIX)/GDI
vpath %.cpp $(SRCDIR)/$(PREFIX)/IO
vpath %.cpp $(SRCDIR)/$(PREFIX)/Net
vpath %.cpp $(SRCDIR)/$(PREFIX)/Media
vpath %.cpp $(SRCDIR)/$(PREFIX)/Media/V4L2

# pending
vpath %.c $(THRDIR)/libz/source
vpath %.c $(THRDIR)/libjpeg/source
vpath %.c $(THRDIR)/libtiff/source
vpath %.c $(THRDIR)/libpng/source-$(PNG_VER)

SRCS = \
xie_high.cpp

SRCS += \
api_file_plugins.cpp \
CxFilePluginJpeg.cpp \
CxFilePluginJpeg_Check.cpp \
CxFilePluginJpeg_Load.cpp \
CxFilePluginJpeg_Save.cpp \
CxFilePluginPng.cpp \
CxFilePluginPng_Check.cpp \
CxFilePluginPng_Load.cpp \
CxFilePluginPng_Save.cpp \
CxFilePluginTiff.cpp \
CxFilePluginTiff_Check.cpp \
CxFilePluginTiff_Load.cpp \
CxFilePluginTiff_Save.cpp \
fnXIE_File_CopyJpegMarkers.cpp

SRCS += \
api_gdi.cpp \
api_gdi_exports.cpp \
api_gdi_graphics.cpp \
AxiGDIDiagnostics.cpp \
CxBitmap.cpp \
CxCanvas.cpp \
CxGdiCircle.cpp \
CxGdiCircleArc.cpp \
CxGdiEllipse.cpp \
CxGdiEllipseArc.cpp \
CxGdiImage.cpp \
CxGdiLine.cpp \
CxGdiLineSegment.cpp \
CxGdiPoint.cpp \
CxGdiPolyline.cpp \
CxGdiRectangle.cpp \
CxGdiStringA.cpp \
CxGdiStringW.cpp \
CxGdiTrapezoid.cpp \
CxGdiVoid.cpp \
CxGraphics.cpp \
CxOverlay.cpp \
CxOverlayGrid.cpp \
CxOverlayProfile.cpp \
CxTexture.cpp \
fnPRV_GDI_Extract.cpp \
fnPRV_GDI_ShrinkHT.cpp \
fnPRV_GDI_ShrinkNN.cpp \
TxBitmap.cpp \
TxCanvas.cpp \
TxGdi2dParam.cpp \
TxGdiImage.cpp \
TxGdiPolyline.cpp \
TxGdiStringA.cpp \
TxGdiStringW.cpp \
TxHitPosition.cpp

SRCS += \
api_io.cpp \
api_io_exports.cpp \
AxiIODiagnostics.cpp \
CxSerialPort.cpp \
TxSerialPort.cpp

SRCS += \
api_media.cpp \
api_media_exports.cpp \
AxiMediaDiagnostics.cpp \
CxCamera.cpp \
CxControlProperty.cpp \
CxDeviceList.cpp \
CxDeviceListItem.cpp \
CxDeviceParam.cpp \
CxGrabber.cpp \
CxGrabberArgs.cpp \
CxMediaPlayer.cpp \
CxScreenCapture.cpp \
CxScreenList.cpp \
CxScreenListItem.cpp \
TxDeviceListItem.cpp \
TxDeviceParam.cpp \
TxGrabberArgs.cpp \
TxScreenListItem.cpp

SRCS += \
api_v4l2.cpp \
CxMMap.cpp \
CxVLCamera.cpp \
CxVLControlProperty.cpp \
CxVLGrabber.cpp \
CxVLGrabberArgs.cpp \
CxVLMediaPlayer.cpp \
CxVLSampleReceiver.cpp \
CxVLScreenCapture.cpp

SRCS += \
api_net.cpp \
api_net_exports.cpp \
CxTcpClient.cpp \
CxTcpServer.cpp \
CxUdpClient.cpp \
TxIPAddress.cpp \
TxIPEndPoint.cpp \
TxSocketStream.cpp \
TxTcpClient.cpp \
TxTcpServer.cpp \
TxUdpClient.cpp

# pending
Z_SRCS = \
adler32.c \
compress.c \
crc32.c \
deflate.c \
gzclose.c \
gzlib.c \
gzread.c \
gzwrite.c \
infback.c \
inffast.c \
inflate.c \
inftrees.c \
trees.c \
uncompr.c \
zutil.c 

# pending
PNG_SRCS = \
png.c \
pngerror.c \
pngget.c \
pngmem.c \
pngpread.c \
pngread.c \
pngrio.c \
pngrtran.c \
pngrutil.c \
pngset.c \
pngtrans.c \
pngwio.c \
pngwrite.c \
pngwtran.c \
pngwutil.c

# pending
JPEG_SRCS = \
jaricom.c \
jcapimin.c \
jcapistd.c \
jcarith.c \
jccoefct.c \
jccolor.c \
jcdctmgr.c \
jchuff.c \
jcinit.c \
jcmainct.c \
jcmarker.c \
jcmaster.c \
jcomapi.c \
jcparam.c \
jcprepct.c \
jcsample.c \
jctrans.c \
jdapimin.c \
jdapistd.c \
jdarith.c \
jdatadst.c \
jdatasrc.c \
jdcoefct.c \
jdcolor.c \
jddctmgr.c \
jdhuff.c \
jdinput.c \
jdmainct.c \
jdmarker.c \
jdmaster.c \
jdmerge.c \
jdpostct.c \
jdsample.c \
jdtrans.c \
jerror.c \
jfdctflt.c \
jfdctfst.c \
jfdctint.c \
jidctflt.c \
jidctfst.c \
jidctint.c \
jmemmgr.c \
jmemnobs.c \
jquant1.c \
jquant2.c \
jutils.c

# pending
TIFF_SRCS = \
tif_aux.c \
tif_close.c \
tif_codec.c \
tif_color.c \
tif_compress.c \
tif_dir.c \
tif_dirinfo.c \
tif_dirread.c \
tif_dirwrite.c \
tif_dumpmode.c \
tif_error.c \
tif_extension.c \
tif_fax3.c \
tif_fax3sm.c \
tif_flush.c \
tif_getimage.c \
tif_jbig.c \
tif_jpeg.c \
tif_jpeg_12.c \
tif_luv.c \
tif_lzma.c \
tif_lzw.c \
tif_next.c \
tif_ojpeg.c \
tif_open.c \
tif_packbits.c \
tif_pixarlog.c \
tif_predict.c \
tif_print.c \
tif_read.c \
tif_strip.c \
tif_swab.c \
tif_thunder.c \
tif_tile.c \
tif_version.c \
tif_warning.c \
tif_unix.c \
tif_write.c \
tif_zip.c 
