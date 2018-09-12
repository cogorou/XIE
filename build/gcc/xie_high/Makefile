#

PREFIX = xie_high
ifeq ($(DEBUG),1)
	TARGET = lib$(PREFIX)d.so
	XIE_CORE = xie_cored
	LIBZ = libzd.a
	LIBPNG = libpngd.a
	LIBJPEG = libjpegd.a
	LIBTIFF = libtiffd.a
else
	TARGET = lib$(PREFIX).so
	XIE_CORE = xie_core
	LIBZ = libz.a
	LIBPNG = libpng.a
	LIBJPEG = libjpeg.a
	LIBTIFF = libtiff.a
endif

OS_ARCH = $(shell uname -m)
TOPDIR = ../../..
SRCDIR = ../../../source
INCDIR = ../../../include
LIBDIR = ../../../lib
THRDIR = ../../../thirdparty
PNG_VER=1.2.50
#PNG_VER=1.6.6

#CROSS_COMPILE=arm-linux-gnueabihf-
CC  = $(CROSS_COMPILE)gcc
CXX = $(CROSS_COMPILE)g++
CFLAGS   = -fPIC -Wall -Wno-unknown-pragmas -Wno-unused-variable -Wno-unused-but-set-variable
CXXFLAGS = -fPIC -Wall -Wno-unknown-pragmas -Wno-unused-variable -Wno-unused-but-set-variable -fpermissive -std=c++11
CDEFS    = -DLINUX -DXIE_EXPLICIT_INCLUDE
LDFLAGS  = -L$(LIBDIR) -l$(XIE_CORE) -lm -lX11 -lGL -lGLU -lGLEW
ARCHIVES = 
INCLUDES = \
-I$(SRCDIR)/$(PREFIX) \
-I$(INCDIR)

# pending
#-I$(THRDIR)/libz/source \
#-I$(THRDIR)/libpng/source-$(PNG_VER) \
#-I$(THRDIR)/libjpeg/source \
#-I$(THRDIR)/libjpeg/include/gcc \
#-I$(THRDIR)/libtiff/source \
#

ifeq ($(DEBUG),1)
	OBJDIR    = objd
	OSSDIR    = ossd
	CFLAGS   += -O0 -g
	CXXFLAGS += -O0 -g
	CDEFS    += -DDEBUG -D_DEBUG
else
	OBJDIR    = obj
	OSSDIR    = oss
	CFLAGS   += -O3
	CXXFLAGS += -O3
endif

# for CUDA
CUDA_PATH = ../../../thirdparty/CUDA/v7.0/gcc
CUDA_RT = $(wildcard $(CUDA_PATH)/lib/$(OS_ARCH)/libcudart_static.a)

ifneq ("$(CUDA_RT)","")
	CDEFS    += -DUSE_CUDA
	INCLUDES += -I$(CUDA_PATH)/include
	ARCHIVES += $(CUDA_RT) 
endif

.PHONY: all clean
.SUFFIXES: .c .cpp .o

# include source list
include Makefile.inc

vpath %.o $(OBJDIR)

# pending
vpath %.o $(OSSDIR)

OBJS = $(addprefix $(OBJDIR)/, $(subst .cpp,.o,$(SRCS)))

# pending
Z_OBJ = $(addprefix $(OSSDIR)/, $(subst .c,.o,$(Z_SRCS)))
PNG_OBJ = $(addprefix $(OSSDIR)/, $(subst .c,.o,$(PNG_SRCS)))
JPEG_OBJ = $(addprefix $(OSSDIR)/, $(subst .c,.o,$(JPEG_SRCS)))
TIFF_OBJ = $(addprefix $(OSSDIR)/, $(subst .c,.o,$(TIFF_SRCS)))

all: $(OBJDIR) $(OBJS)
	$(CC) -shared -Wl,-soname,$(TARGET) -o $(TARGET) $(OBJS) -Wl,--whole-archive $(ARCHIVES) -Wl,--no-whole-archive $(LDFLAGS)

# pending
#all: $(OBJDIR) $(OSSDIR) $(OBJS) $(LIBZ) $(LIBPNG) $(LIBJPEG) $(LIBTIFF)
#	$(CC) -shared -Wl,-soname,$(TARGET) -o $(TARGET) $(OBJS) -Wl,--whole-archive $(LIBZ) $(LIBPNG) $(LIBJPEG) $(LIBTIFF) $(ARCHIVES) -Wl,--no-whole-archive $(LDFLAGS)
#$(LIBZ) : $(Z_OBJ)
#	$(AR) csrv $(LIBZ) $(Z_OBJ)
#$(LIBPNG) : $(PNG_OBJ)
#	$(AR) csrv $(LIBPNG) $(PNG_OBJ)
#$(LIBJPEG) : $(JPEG_OBJ)
#	$(AR) csrv $(LIBJPEG) $(JPEG_OBJ)
#$(LIBTIFF) : $(TIFF_OBJ)
#	$(AR) csrv $(LIBTIFF) $(TIFF_OBJ)

install: all $(LIBDIR)
	cp -a $(TARGET) $(LIBDIR)/.

$(LIBDIR):
	mkdir -p $(LIBDIR)

$(OBJDIR):
	mkdir -p $(OBJDIR)

# pending
#$(OSSDIR):
#	mkdir -p $(OSSDIR)

clean:
	$(RM) $(TARGET)
	rm -rf $(OBJDIR)

# pending
ifneq ("$(wildcard $(OSSDIR))","")
	rm -rf $(OSSDIR)
endif
ifneq ("$(wildcard $(LIBZ))","")
	$(RM) $(LIBZ)
endif
ifneq ("$(wildcard $(LIBPNG))","")
	$(RM) $(LIBPNG)
endif
ifneq ("$(wildcard $(LIBJPEG))","")
	$(RM) $(LIBJPEG)
endif
ifneq ("$(wildcard $(LIBTIFF))","")
	$(RM) $(LIBTIFF)
endif

$(OBJDIR)/%.o : %.cpp
	$(CXX) $(CXXFLAGS) $(CDEFS) $(INCLUDES) -o $@ -c $^

$(OBJDIR)/%.o : %.c
	$(CC) $(CFLAGS) $(CDEFS) $(INCLUDES) -o $@ -c $^

# pending
#$(OSSDIR)/%.o : %.c
#	$(CC) $(CFLAGS) $(CDEFS) $(INCLUDES) -o $@ -c $^
