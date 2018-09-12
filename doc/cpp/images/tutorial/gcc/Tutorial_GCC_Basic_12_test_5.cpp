if (xserver != NULL)
{
	printf( "Dispose\n" );
	
	canvas.Dispose();
	
	if (gc != 0)
		XFreeGC( xserver, gc );
	if (window != 0)
		XDestroyWindow( xserver, window );
	XCloseDisplay( xserver );
}
