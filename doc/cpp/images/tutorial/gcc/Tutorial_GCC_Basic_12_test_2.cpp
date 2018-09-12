// X ディスプレイサーバーに接続する.
printf( "XOpenDisplay\n" );
if (xserver == NULL)
	xserver = XOpenDisplay( NULL );	// 環境変数に従う.
if (xserver == NULL)
	xserver = XOpenDisplay( ":0" );	// ローカル.
if (xserver == NULL)
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
