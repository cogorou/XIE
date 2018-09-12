
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
// global variable
static xie::CxMutex g_mutex;	// mutex object

// prototype
static void _func1_(void* sender, xie::CxThreadArgs* e);
static void _func2_(void* sender, xie::CxThreadArgs* e);

// ============================================================
void CxMutex_01()
{
	printf("%s\n", __FUNCTION__);

	// shared variable
	int ans = 0;

	// thread
	xie::CxThread thread1;
	xie::CxThread thread2;

	// initialize thread
	thread1.Notify = xie::CxThreadEvent(_func1_);
	thread1.Param = &ans;
	thread1.Setup();
	thread2.Notify = xie::CxThreadEvent(_func2_);
	thread2.Param = &ans;
	thread2.Setup();

	// start thread
	printf("Start\n");
	{
		g_mutex.Lock();			// (1) Lock
		thread1.Start();
		thread2.Start();

		// wait until thread1 finish.
		while(thread1.IsRunning())
			xie::Axi::Sleep(1);

		// thread2 will start.
		g_mutex.Unlock();		// (2) Unlock
		while(thread2.IsRunning())
			xie::Axi::Sleep(1);
	}
	printf("Finish\n");

	// release thread
	thread1.Dispose();
	thread2.Dispose();
}

// ============================================================
static void _func1_(void* sender, xie::CxThreadArgs* e)
{
	int& ans = *static_cast<int*>(e->Param);

	// 1,2,...,10
	for(int i=0 ; i<10 ; i++)
	{
		ans++;
		printf("thread1: ans = %d\n", ans);
		xie::Axi::Sleep(10);
	}
	e->Cancellation = true;		// true=stop, false=continue
}

// ============================================================
static void _func2_(void* sender, xie::CxThreadArgs* e)
{
	int& ans = *static_cast<int*>(e->Param);

	g_mutex.Lock();				// Locked by (1), Unloack by (2)
	for(int i=0 ; i<5 ; i++)
	{
		ans *= 2;
		printf("thread2: ans = %d\n", ans);
		xie::Axi::Sleep(100);
	}
	g_mutex.Unlock();			// You must always unlock the mutex.
	e->Cancellation = true;		// true=stop, false=continue
}

}
