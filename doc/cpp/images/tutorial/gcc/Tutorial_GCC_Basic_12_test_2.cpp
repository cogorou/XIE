// X �f�B�X�v���C�T�[�o�[�ɐڑ�����.
printf( "XOpenDisplay\n" );
if (xserver == NULL)
	xserver = XOpenDisplay( NULL );	// ���ϐ��ɏ]��.
if (xserver == NULL)
	xserver = XOpenDisplay( ":0" );	// ���[�J��.
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
