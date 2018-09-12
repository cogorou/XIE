
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================

// ============================================================
void CxThread_01()
{
	printf("%s\n", __FUNCTION__);

	int counter = 0;

	// thread
	xie::CxThread thread;

	// initialize thread
	// (1)
	thread.Notify = xie::CxThreadEvent([](void* sender, xie::CxThreadArgs* e)
		{
			auto pthread = (xie::CxThread*)sender;
			auto pcounter = (int*)e->Param;
			for(int i=0 ; i<10 ; i++)
			{
				(*pcounter)++;
				printf("hello! (%d)\n", i);
				xie::Axi::Sleep(10);
			}
			e->Cancellation = true;	// true=stop, false=continue
		});
	// (2)
	thread.Param = &counter;
	thread.Setup();

	// start thread
	printf("Start\n");
	thread.Start();
	if (thread.Wait(3000) == false)	// 3,000 msec.
		thread.Stop();
	printf("Finish (counter=%d)\n", counter);

	// release thread
	thread.Dispose();
}

}
