/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"		// Setup/TearDown

const int None		= 0;
const int True		= 1;
const int False		= 0;
const int Success	= 0;

// ============================================================
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
static Bool WindowIsMapped(
	Display*	xserver,
	XEvent*		xevent,
	XPointer	args
)
{
	if( xevent->type == MapNotify )
		return True;
	else
		return False;
}

// =======================================================
void test()
{
	xie::Axi::Setup();
	
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
	// �ڑ�.
	{
		// X�T�[�o�[�ɐڑ�����.(NULL=�f�t�H���g)
		printf( "XOpenDisplay\n" );
		if (xserver == NULL)
			xserver = XOpenDisplay( NULL );
		if (xserver == NULL)
			xserver = XOpenDisplay( ":0" );
		if (xserver == NULL)
		{
			printf( "ERR(%d): XOpenDisplay\n", __LINE__ );
			throw xie::CxException(xie::ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}

		// �r�W���A�����.
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
	// �E�B���h�E�쐬.
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
					0, 0,			// �{�[�_�[�̕��ƐF.
					ulBG			// �E�B���h�E�̔w�i�F.
				);
		
		// �O���t�B�b�N�R���e�L�X�g�𐶐�����.
		gc = XCreateGC( xserver, window, 0, 0 );

		XSetForeground( xserver, gc, ulFG );
		XSetBackground( xserver, gc, ulBG );

		// �E�B���h�E�^�C�g��.
		printf( "XStoreName\n" );
		XStoreName( xserver, window, szWindowTitle );

		// �E�B���h�E�̈ʒu�A�傫�����w�� 
		XSizeHints	xshint;
		xshint.x = window_x;
		xshint.y = window_y;
		xshint.width = window_w;
		xshint.height = window_h;
		xshint.flags = PPosition | PSize;

		// ���������E�B���h�E�̏����V�X�e���ɒʒm����.
		XSetWMNormalHints( xserver, window, &xshint );

		// �\�����e���N���A.
		XClearWindow( xserver, window );

		// �C�x���g�}�X�N���w��.(StructureNotifyMask=MapNotify�C�x���g�����o.)
		eventInfo.unMask = (StructureNotifyMask|ButtonPressMask|ButtonReleaseMask|KeyPressMask|ExposureMask);
		XSelectInput( xserver, window, eventInfo.unMask );

		// �E�B���h�E�����̉�.
		XMapWindow( xserver, window );

		// �E�B���h�E���}�b�v�����܂őҋ@.
		XEvent xevent;
		XIfEvent( xserver, &xevent, WindowIsMapped, NULL );

		// XServer �ɏo��.
		XSync( xserver, False );
		
		// Flush
		XFlush( xserver );
	}
	
	// ----------------------------------------------------------------------
	// �ҋ@.
	{
		printf( "Wait\n" );
		
		// XIE �f�B�X�v���C.
		xie::GDI::CxCanvas	canvas;
		
		// �摜�f�[�^.
		xie::CxImage	image("TestFiles/bird.jpg", false);
		
		bool	alived = true;
		while( alived )
		{
			XEvent	event;	// �C�x���g�\����.
			XNextEvent( xserver, &event );
			switch ( event.type )
			{
				case KeyPress:			// �L�[�{�[�h.
					alived = false;
					break;
				case ConfigureNotify:	// �ύX.
					{
						XConfigureEvent config = event.xconfigure;
						window_w = config.width;
						window_h = config.height;
					}
					break;
				case Expose:			// �ĕ`��.
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
						{
							xie::GDI::CxGdiRectangle rect(100, 50, 200, 100);
							rect.PenColor(xie::TxRGB8x4(255, 0, 0, 128));
							rect.PenStyle(xie::GDI::ExGdiPenStyle::Solid);
							rect.PenWidth(3);
							rect.BkColor(xie::TxRGB8x4(0, 255, 0, 64));
							rect.BkEnable(true);
							canvas.DrawOverlay(rect, xie::GDI::ExGdiScalingMode::TopLeft);
						}
						
						// 5) flush
						canvas.Flush();
					}
					break;
			}
		}
   	}

	// ----------------------------------------------------------------------
	// ���.
	if (xserver != NULL)
	{
		printf( "Dispose\n" );
		
		if (gc != 0)
			XFreeGC( xserver, gc );
		if (window != 0)
			XDestroyWindow( xserver, window );
		XCloseDisplay( xserver );
	}
	
	xie::Axi::TearDown();
}
