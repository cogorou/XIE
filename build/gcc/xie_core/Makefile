#

PREFIX = xie_core
ifeq ($(DEBUG),1)
	TARGET = lib$(PREFIX)d.so
else
	TARGET = lib$(PREFIX).so
endif
TOPDIR = ../../..
SRCDIR = ../../../source
INCDIR = ../../../include
LIBDIR = ../../../lib

#CROSS_COMPILE=arm-linux-gnueabihf-
CC  = $(CROSS_COMPILE)gcc
CXX = $(CROSS_COMPILE)g++
CFLAGS   = -fPIC -Wall -Wno-unknown-pragmas -Wno-unused-variable -Wno-unused-but-set-variable
CXXFLAGS = -fPIC -Wall -Wno-unknown-pragmas -Wno-unused-variable -Wno-unused-but-set-variable -fpermissive -std=c++11
CDEFS    = -DLINUX -DXIE_EXPLICIT_INCLUDE
LDFLAGS  = -lm -lstdc++ -lpthread -lrt
ARCHIVES = 
INCLUDES = \
-I$(SRCDIR)/$(PREFIX) \
-I$(INCDIR)

ifeq ($(DEBUG),1)
	OBJDIR    = objd
	CFLAGS   += -O0 -g
	CXXFLAGS += -O0 -g
	CDEFS    += -DDEBUG -D_DEBUG
else
	OBJDIR    = obj
	CFLAGS   += -O3
	CXXFLAGS += -O3
endif

.PHONY: all clean
.SUFFIXES: .c .cpp .o

# include source list
include Makefile.inc

vpath %.o $(OBJDIR)

OBJS = $(addprefix $(OBJDIR)/, $(subst .cpp,.o,$(SRCS)))

all: $(OBJDIR) $(OBJS)
	$(CC) -shared -Wl,-soname,$(TARGET) -o $(TARGET) $(OBJS) $(ARCHIVES) $(LDFLAGS)

install: all $(LIBDIR)
	cp -a $(TARGET) $(LIBDIR)/.

$(LIBDIR):
	mkdir -p $(LIBDIR)

$(OBJDIR):
	mkdir -p $(OBJDIR)

clean:
	$(RM) $(TARGET)
	rm -rf $(OBJDIR)

$(OBJDIR)/%.o : %.c
	$(CC) $(CFLAGS) $(CDEFS) $(INCLUDES) -o $@ -c $^

$(OBJDIR)/%.o : %.cpp
	$(CXX) $(CXXFLAGS) $(CDEFS) $(INCLUDES) -o $@ -c $^
