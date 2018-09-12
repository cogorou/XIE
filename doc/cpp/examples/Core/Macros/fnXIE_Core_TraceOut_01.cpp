
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void fnXIE_Core_TraceOut_01()
{
	xie::Axi::TraceLevel(2);

	printf("trace=%d\n", xie::Axi::TraceLevel());

	xie::fnXIE_Core_TraceOut(1, "level=%d\n", 1);
	xie::fnXIE_Core_TraceOut(2, "level=%d\n", 2);
	xie::fnXIE_Core_TraceOut(3, "level=%d\n", 3);

	xie::Axi::TraceLevel(0);
}

}
