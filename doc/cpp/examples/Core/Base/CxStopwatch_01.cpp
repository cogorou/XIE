
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxStopwatch_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxStopwatch watch;

	watch.Start();
	::Sleep(100);
	watch.Stop();
	printf("Lap=%9.3f msec, Elapsed=%9.3f msec\n", watch.Lap, watch.Elapsed);

	watch.Start();
	::Sleep(200);
	watch.Stop();
	printf("Lap=%9.3f msec, Elapsed=%9.3f msec\n", watch.Lap, watch.Elapsed);
}

}
