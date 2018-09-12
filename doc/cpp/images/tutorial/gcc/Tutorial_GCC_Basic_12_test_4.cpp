printf( "Wait\n" );

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
				canvas.Resize(window_w, window_h);
				
				// 3) image
				canvas.DrawImage(image);
				
				// 4) overlay
				{
					xie::GDI::CxGdiRectangle rect(100, 50, 200, 100);
					rect.PenColor(xie::TxRGB8x4(255, 0, 0, 128));
					rect.PenStyle(xie::GDI::ExGdiPenStyle_Solid);
					rect.PenWidth(3);
					rect.BkColor(xie::TxRGB8x4(0, 255, 0, 64));
					rect.BkEnable(true);
					canvas.DrawOverlay(rect, xie::GDI::ExGdiScalingMode_TopLeft);
				}
				
				// 5) flush
				canvas.Flush();
			}
			break;
	}
}
