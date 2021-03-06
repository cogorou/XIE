#

PREFIX = demo
TARGET = $(PREFIX)

ifeq ($(DEBUG),1)
	XIE_CORE = xie_cored
	XIE_HIGH = xie_highd
else
	XIE_CORE = xie_core
	XIE_HIGH = xie_high
endif

XIE_TOP=../../../../../../XIE
XIE_INC=$(XIE_TOP)/include
XIE_LIB=$(XIE_TOP)/lib

#CROSS_COMPILE=arm-linux-gnueabihf-
CC  = $(CROSS_COMPILE)gcc
CXX = $(CROSS_COMPILE)g++
CFLAGS   = -Wall -Wno-unknown-pragmas -Wno-unused-variable
CXXFLAGS = -Wall -Wno-unknown-pragmas -Wno-unused-variable -fpermissive -std=c++11
CDEFS    = -DLINUX
LDFLAGS  = -L$(XIE_LIB) -l$(XIE_CORE) -l$(XIE_HIGH) -lX11 -lGL -lGLU -lGLEW -lstdc++
ARCHIVES = 
INCDIR   = \
-I$(XIE_INC)

ifeq ($(DEBUG),1)
	CFLAGS   += -O0 -g
	CXXFLAGS += -O0 -g
	CDEFS    += -DDEBUG -D_DEBUG
else
	CFLAGS   += -O3
	CXXFLAGS += -O3
endif

.PHONY: all clean
.SUFFIXES: .c .cpp .o

vpath %.cpp source
vpath %.o obj

OBJDIR=obj

SRCS = \
main.cpp \
test.cpp


OBJS = $(addprefix $(OBJDIR)/, $(subst .cpp,.o,$(SRCS)))

all: $(OBJDIR) $(OBJS) Results
	$(CC) -o $(TARGET) $(OBJS) $(ARCHIVES) $(LDFLAGS)

$(OBJDIR):
	mkdir -p $(OBJDIR)

Results:
	mkdir -p Results

clean:
	$(RM) $(TARGET)
	$(RM) $(OBJS)
	$(RM) -rf $(OBJDIR)
	$(RM) -rf Results

$(OBJDIR)/%.o : %.c
	$(CC) $(CFLAGS) $(CDEFS) $(INCDIR) -o $@ -c $^

$(OBJDIR)/%.o : %.cpp
	$(CXX) $(CXXFLAGS) $(CDEFS) $(INCDIR) -o $@ -c $^
