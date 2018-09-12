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

// �E�B���h�E�̈ʒu�A�傫�����w�肷��.
XSizeHints	xshint;
xshint.x = window_x;
xshint.y = window_y;
xshint.width = window_w;
xshint.height = window_h;
xshint.flags = PPosition | PSize;

// ���������E�B���h�E�̏����V�X�e���ɒʒm����.
XSetWMNormalHints( xserver, window, &xshint );

// �\�����e���N���A����.
XClearWindow( xserver, window );

// �C�x���g�}�X�N���w�肷��.(StructureNotifyMask=MapNotify �C�x���g�����o����.)
eventInfo.unMask = (StructureNotifyMask|ButtonPressMask|ButtonReleaseMask|KeyPressMask|ExposureMask);
XSelectInput( xserver, window, eventInfo.unMask );

// �E�B���h�E�����̉�����.
XMapWindow( xserver, window );

// �E�B���h�E���}�b�v�����܂őҋ@����.
XEvent xevent;
XIfEvent( xserver, &xevent, WindowIsMapped, NULL );

// XServer �ɏo�͂���.
XSync( xserver, False );

// Flush
XFlush( xserver );
