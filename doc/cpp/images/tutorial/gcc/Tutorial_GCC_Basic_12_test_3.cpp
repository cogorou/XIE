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

// ウィンドウの位置、大きさを指定する.
XSizeHints	xshint;
xshint.x = window_x;
xshint.y = window_y;
xshint.width = window_w;
xshint.height = window_h;
xshint.flags = PPosition | PSize;

// 生成したウィンドウの情報をシステムに通知する.
XSetWMNormalHints( xserver, window, &xshint );

// 表示内容をクリアする.
XClearWindow( xserver, window );

// イベントマスクを指定する.(StructureNotifyMask=MapNotify イベントを検出する.)
eventInfo.unMask = (StructureNotifyMask|ButtonPressMask|ButtonReleaseMask|KeyPressMask|ExposureMask);
XSelectInput( xserver, window, eventInfo.unMask );

// ウィンドウを実体化する.
XMapWindow( xserver, window );

// ウィンドウがマップされるまで待機する.
XEvent xevent;
XIfEvent( xserver, &xevent, WindowIsMapped, NULL );

// XServer に出力する.
XSync( xserver, False );

// Flush
XFlush( xserver );
