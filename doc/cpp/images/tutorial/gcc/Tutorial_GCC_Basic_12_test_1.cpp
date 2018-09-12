const char* szWindowTitle = "viewer";

Display*	xserver = NULL;

int			screen = 0;
GC			gc = 0;
Window		window = 0;
int			window_x = 0;
int			window_y = 0;
int			window_w = 640;
int			window_h = 480;
bool		alived = true;

EventInfoS	eventInfo;
{
	eventInfo.blMouseGrip = False;
	eventInfo.nOffsetX = 0;
	eventInfo.nOffsetY = 0;
	eventInfo.nPressX = 0;
	eventInfo.nPressY = 0;
	eventInfo.unMask = NoEventMask;
}

// 画像描画オブジェクト.
xie::GDI::CxCanvas	canvas;

// 画像データ.
xie::CxImage	image("TestFiles/bird.jpg", false);
