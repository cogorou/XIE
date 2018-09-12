#

vpath %.cpp $(SRCDIR)/$(PREFIX)
vpath %.cpp $(SRCDIR)/$(PREFIX)/Base
vpath %.cpp $(SRCDIR)/$(PREFIX)/Data
vpath %.cpp $(SRCDIR)/$(PREFIX)/Effectors
vpath %.cpp $(SRCDIR)/$(PREFIX)/Exchange
vpath %.cpp $(SRCDIR)/$(PREFIX)/Exif
vpath %.cpp $(SRCDIR)/$(PREFIX)/File
vpath %.cpp $(SRCDIR)/$(PREFIX)/Structures

SRCS = \
xie_core.cpp

SRCS += \
Axi.cpp \
AxiDiagnostics.cpp \
CxException.cpp \
CxModule.cpp \
CxMutex.cpp \
CxStopwatch.cpp \
CxThread.cpp \
CxThreadArgs.cpp

SRCS += \
api_data.cpp \
CxArray.cpp \
CxArrayFilter.cpp \
CxImage.cpp \
CxImageFilter.cpp \
CxMatrix.cpp \
CxMatrixFilter.cpp \
CxString.cpp \
CxStringA.cpp \
CxStringW.cpp \
fnPRV_Core_2D_Affine.cpp \
fnPRV_Core_2D_Cast.cpp \
fnPRV_Core_2D_Clear.cpp \
fnPRV_Core_2D_ClearEx.cpp \
fnPRV_Core_2D_ColorMatrix.cpp \
fnPRV_Core_2D_Compare.cpp \
fnPRV_Core_2D_Convert.cpp \
fnPRV_Core_2D_Copy.cpp \
fnPRV_Core_2D_CopyEx.cpp \
fnPRV_Core_2D_Extract.cpp \
fnPRV_Core_2D_GeoTrans.cpp \
fnPRV_Core_2D_Math.cpp \
fnPRV_Core_2D_Not.cpp \
fnPRV_Core_2D_Ope1A.cpp \
fnPRV_Core_2D_Ope1L.cpp \
fnPRV_Core_2D_Ope2A.cpp \
fnPRV_Core_2D_Ope2L.cpp \
fnPRV_Core_2D_RgbToBgr.cpp \
fnPRV_Core_2D_Serialize.cpp \
fnPRV_Core_2D_Statistics.cpp

SRCS += \
CxBinarize1.cpp \
CxBinarize2.cpp \
CxGammaConverter.cpp \
CxHsvConverter.cpp \
CxHsvToRgb.cpp \
CxIntegral.cpp \
CxMonochrome.cpp \
CxPartColor.cpp \
CxProjection.cpp \
CxRgbConverter.cpp \
CxRgbToGray.cpp \
CxRgbToHsv.cpp

SRCS += \
TxCvMat.cpp \
TxIplImage.cpp

SRCS += \
api_exif.cpp \
CxExif.cpp \
TxExif.cpp \
TxExifItem.cpp

SRCS += \
api_file.cpp \
CxFilePluginBmp.cpp \
CxFilePluginBmp_Check.cpp \
CxFilePluginBmp_Load.cpp \
CxFilePluginBmp_Save.cpp \
fnXIE_Core_DIB_Load.cpp \
fnXIE_Core_DIB_Save.cpp

SRCS += \
TxArray.cpp \
TxBGR8x3.cpp \
TxBGR8x4.cpp \
TxCircleD.cpp \
TxCircleI.cpp \
TxCircleArcD.cpp \
TxCircleArcI.cpp \
TxDateTime.cpp \
TxEllipseD.cpp \
TxEllipseI.cpp \
TxEllipseArcD.cpp \
TxEllipseArcI.cpp \
TxFrameIndex.cpp \
TxImage.cpp \
TxImageSize.cpp \
TxLayer.cpp \
TxLineD.cpp \
TxLineI.cpp \
TxLineSegmentD.cpp \
TxLineSegmentI.cpp \
TxMatrix.cpp \
TxModel.cpp \
TxPointD.cpp \
TxPointI.cpp \
TxRangeD.cpp \
TxRangeI.cpp \
TxRawFileHeader.cpp \
TxRectangleD.cpp \
TxRectangleI.cpp \
TxRGB8x3.cpp \
TxRGB8x4.cpp \
TxSizeD.cpp \
TxSizeI.cpp \
TxStatistics.cpp \
TxStringA.cpp \
TxStringW.cpp \
TxTrapezoidD.cpp \
TxTrapezoidI.cpp
