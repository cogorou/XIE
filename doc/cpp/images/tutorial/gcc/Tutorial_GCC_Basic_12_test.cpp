
#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"

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
static Bool WindowIsMapped(Display* canvas, XEvent* xevent, XPointer arg)
{
	return (xevent->type == MapNotify) ? True : False;
}

// =======================================================
void test()
{
	// (1) �ϐ��錾.

	// (2) �ڑ�.

	// (3) �E�B���h�E�쐬.

	// (4) �C�x���g�ҋ@.

	// (5) ���.
}
