/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"

const int None		= 0;
const int True		= 1;
const int False		= 0;
const int Success	= 0;

void test01();

char* szDisplayName = NULL;

// ============================================================
/*!
	@brief	EntryPoint
*/
int main( int argc, char** argv )
{
	xie::Axi::Setup();

	if (argc > 1)
		szDisplayName = argv[1];

	try
	{
		test01();
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
	
	xie::Axi::TearDown();
	
	return 0;
}

// ============================================================
/*!
	@brief	イベント処理用.
*/
typedef struct
{
	int					blMouseGrip;
	int					nOffsetX;
	int					nOffsetY;
	int					nPressX;
	int					nPressY;
	u_int				unMask;
} EventInfoS;

// ============================================================
/*!
	@brief	マップイベントの検査.
	@retval	true	マップ完了
	@retval	fasle	未マップ.
*/
static Bool WindowIsMapped(Display* xserver, XEvent* xevent, XPointer args)
{
	return (xevent->type == MapNotify) ? True : False;
}

// =======================================================
/*!
	@brief	test
*/
void test01()
{
	xie::CxStopwatch watch;
	
	const char* szWindowTitle = "viewer";
	
	Display*	xserver = NULL;

	int			screen = 0;
	GC			gc = 0;
	Window		window = 0;
	int			window_x = 0;
	int			window_y = 0;
	int			window_w = 640;
	int			window_h = 480;
	
	EventInfoS	eventInfo;
	{
		eventInfo.blMouseGrip = False;
		eventInfo.nOffsetX = 0;
		eventInfo.nOffsetY = 0;
		eventInfo.nPressX = 0;
		eventInfo.nPressY = 0;
		eventInfo.unMask = NoEventMask;
	}

	// ----------------------------------------------------------------------
	// 接続.
	{
		// Xサーバーに接続する.(NULL=デフォルト)
		printf( "XOpenDisplay\n" );
		xserver = XOpenDisplay( szDisplayName );
		if ( xserver == NULL )
		{
			printf( "ERR(%d): XOpenDisplay\n", __LINE__ );
			throw xie::CxException(xie::ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}

		// ビジュアル情報.
		printf( "XGetVisualInfo\n" );
		unsigned long	ulXVMask = VisualNoMask;
		XVisualInfo		xvTemp;
		int				nIterms;
		XVisualInfo*	xvInfo = XGetVisualInfo( xserver, ulXVMask, &xvTemp, &nIterms );
		if ( xvInfo == NULL )
		{
			printf( "ERR(%d): XGetVisualInfo\n", __LINE__ );
			throw xie::CxException(xie::ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}
		
		screen = xvInfo->screen;
	}
	
	// ----------------------------------------------------------------------
	// ウィンドウ作成.
	{
		printf( "XCreateSimpleWindow\n" );

		unsigned long	ulFG = WhitePixel( xserver, screen );
		unsigned long	ulBG = BlackPixel( xserver, screen );

		Window parent = RootWindow( xserver, screen );
		window = XCreateSimpleWindow(
					xserver,
					parent,
					window_x,
					window_y,
					window_w,
					window_h,
					0, 0,			// ボーダーの幅と色.
					ulBG			// ウィンドウの背景色.
				);
		
		// グラフィックコンテキストを生成する.
		gc = XCreateGC( xserver, window, 0, 0 );

		XSetForeground( xserver, gc, ulFG );
		XSetBackground( xserver, gc, ulBG );

		// ウィンドウタイトル.
		printf( "XStoreName\n" );
		XStoreName( xserver, window, szWindowTitle );

		// ウィンドウの位置、大きさを指定 
		XSizeHints	xshint;
		xshint.x = window_x;
		xshint.y = window_y;
		xshint.width = window_w;
		xshint.height = window_h;
		xshint.flags = PPosition | PSize;

		// 生成したウィンドウの情報をシステムに通知する.
		XSetWMNormalHints( xserver, window, &xshint );

		// 表示内容をクリア.
		XClearWindow( xserver, window );

		// イベントマスクを指定.(StructureNotifyMask=MapNotifyイベントを検出.)
		eventInfo.unMask = (StructureNotifyMask|ButtonPressMask|ButtonReleaseMask|KeyPressMask|ExposureMask);
		XSelectInput( xserver, window, eventInfo.unMask );

		// ウィンドウを実体化.
		XMapWindow( xserver, window );

		// ウィンドウがマップされるまで待機.
		XEvent xevent;
		XIfEvent( xserver, &xevent, WindowIsMapped, NULL );

		// XServer に出力.
		XSync( xserver, False );
		
		// Flush
		XFlush( xserver );
	}
	
	// ----------------------------------------------------------------------
	// 待機.
	{
		printf( "Wait\n" );
		
		// XIE 画像描画オブジェクト.
		xie::GDI::CxCanvas	canvas;
		
		// 画像データ.
		xie::CxImage	image("TestFiles/cube.png", false);
		xie::CxImage	overlay_image("TestFiles/bird.png", false);
		
		// オーバレイ描画処理1:
		auto draw_overlay1 = [&canvas, &overlay_image]()
		{
			// 全体が半透明の画像.
			{
				xie::GDI::CxGdiImage figure(overlay_image);
				figure.Location(xie::TxPointD(320, 240));
				figure.Alpha(0.5);
				figure.AlphaFormat(false);

				canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::TopLeft);
			}

			// 
			{
				std::vector<xie::GDI::CxGdiRectangle>	figures;

				// 赤い実線の矩形.(傾きあり)
				{
					xie::GDI::CxGdiRectangle figure(10, 20, 100, 50);
					figure.PenColor(xie::TxRGB8x4(255, 0, 0));			// 赤.
					figure.PenStyle(xie::GDI::ExGdiPenStyle::Solid);	// 実線.
					figure.PenWidth(1);
					figure.Angle(45);									// +45度(右回転)
					figure.BkColor(xie::TxRGB8x4(255, 0, 0, 64));		// 赤 (半透明) 64=不透明度25%
					figure.BkEnable(true);
					figures.push_back(figure);
				}

				// 青い破線の矩形.
				{
					xie::GDI::CxGdiRectangle figure(320, 240, 320, 240);
					figure.PenColor(xie::TxRGB8x4(0, 0, 255));			// 青.
					figure.PenStyle(xie::GDI::ExGdiPenStyle::Dash);		// 破線.
					figure.PenWidth(1);
					figure.Angle(0.0);
					figures.push_back(figure);
				}

				canvas.DrawOverlay(figures, xie::GDI::ExGdiScalingMode::TopLeft);
			}

			// 緑色の矩形と文字列.(傾きあり)
			{
				xie::TxRectangleD rect(100, 240, 100, 50);
				double angle = -15;		// -15度(左回転)

				// 緑色の実線の矩形.
				{
					xie::GDI::CxGdiRectangle figure(rect);
					figure.PenColor(xie::TxRGB8x4(0, 255, 0));			// 緑色.
					figure.PenStyle(xie::GDI::ExGdiPenStyle::Solid);	// 実線.
					figure.PenWidth(3);
					figure.Angle(angle);

					canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::TopLeft);
				}

				std::vector<xie::GDI::CxGdiStringA>	figures;

				// 矩形の上辺(横線)に平行な文字列.
				{
					xie::GDI::CxGdiStringA figure;
					figure.Text(xie::CxString::Format("<- %.0f ->", rect.Width));
					figure.Location(xie::TxPointD(rect.X, rect.Y-4));
					figure.Align(xie::GDI::ExGdiTextAlign::BottomLeft);
				//	figure.CodePage(932);				// Text に指定した文字列が化けない為の対策.
				//	figure.FontSize( 24 );
					figure.FontName("-misc-fixed-medium-r-normal--20-*");
					figure.Angle(angle);
					figure.PenColor(xie::TxRGB8x4(0xFF, 0x00, 0x00));
					figure.PenWidth(4);
					figures.push_back(figure);
				}

				// 矩形の左辺(縦線)に平行な文字列.
				{
					xie::GDI::CxGdiStringA figure;
					figure.Text(xie::CxString::Format("<- %.0f ->", rect.Height));
					figure.Location(xie::TxPointD(rect.X-4, rect.Y));
					figure.Align(xie::GDI::ExGdiTextAlign::TopLeft);
				//	figure.CodePage(932);				// Text に指定した文字列が化けない為の対策.
				//	figure.FontSize( 14 );
					figure.FontName("-misc-fixed-medium-r-normal--14-*");
					figure.Angle(90.0 + angle);
					figure.PenColor(xie::TxRGB8x4(0xFF, 0x00, 0x00));
					figure.PenWidth(4);
					figures.push_back(figure);
				}

				canvas.DrawOverlay(figures, xie::GDI::ExGdiScalingMode::TopLeft);
			}
		};

		// オーバレイ描画処理2:
		auto draw_overlay2 = [&canvas, &overlay_image]()
		{
			// 黄色い文字列.
			{
				xie::GDI::CxGdiStringA figure;
			//	figure.Text("ハロー世界♪");
				figure.Text("Hello, World!!");
				figure.Location(xie::TxPointD(0, 0));
				figure.Align(xie::GDI::ExGdiTextAlign::TopLeft);
			//	figure.CodePage(0);
				figure.FontName("fixed");
				figure.PenColor(xie::TxRGB8x4(0xFF, 0xFF, 0x00));

				canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::None);
			}

			// 画像 (画素毎の透過あり) (傾きあり)
			{
				xie::GDI::CxGdiImage figure(overlay_image);
				figure.Location(xie::TxPointD(640, 480));
				figure.Scanner<xie::TxBGR8x4>().ForEach(
						[](int y, int x, xie::TxBGR8x4* _dst)
						{
							_dst->A = ((_dst->R * 0.299 + _dst->G * 0.587 * _dst->B * 0.114) < 32) ? 0x00 : 0xFF;
						}
					);
				figure.Alpha(1.0);
				figure.AlphaFormat(true);
				figure.Angle(15);	// +15度(右回転)

				canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::None);
			}

			// 画像を囲む Pink の点線. (傾きあり)
			{
				xie::GDI::CxGdiRectangle figure(640, 480, 320, 240);
				figure.PenColor(xie::TxRGB8x4(255, 0, 255));		// Pink.
				figure.PenStyle(xie::GDI::ExGdiPenStyle::Dot);		// 点線.
				figure.PenWidth(1);
				figure.Angle(15);									// +15度(右回転)

				canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::None);
			}
		};
		
		bool	alived = true;
		while( alived )
		{
			XEvent	event;	// イベント構造体.
			XNextEvent( xserver, &event );
			switch ( event.type )
			{
				case KeyPress:			// キーボード.
					alived = false;
					break;
				case ConfigureNotify:	// 変更.
					{
						XConfigureEvent config = event.xconfigure;
						window_w = config.width;
						window_h = config.height;
					}
					break;
				case Expose:			// 再描画.
					{
						// 1) setup
						canvas.Setup(window);
						
						// 2) resize
						if (window_w != canvas.Width() ||
							window_h != canvas.Height() )
						{
							canvas.Resize(window_w, window_h);
						}
						
						// 3) image
						canvas.DrawImage(image);
						
						// 4) overlay
						draw_overlay1();
						draw_overlay2();
						
						// 5) flush
						canvas.Flush();
					}
					break;
			}
		}
   	}

	// ----------------------------------------------------------------------
	// 解放.
	if (xserver != NULL)
	{
		printf( "Dispose\n" );
		
		if (gc != 0)
			XFreeGC( xserver, gc );
		if (window != 0)
			XDestroyWindow( xserver, window );
		XCloseDisplay( xserver );
	}
}
