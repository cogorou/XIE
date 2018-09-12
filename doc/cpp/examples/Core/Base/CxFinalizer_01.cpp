
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxFinalizer_01()
{
	printf("%s\n", __FUNCTION__);

	try
	{
		printf("(1) allocation\n");
		void* addr = xie::Axi::MemoryAlloc(256 * sizeof(char));
		if (addr == NULL)
			throw xie::CxException(xie::ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		
		printf("(2) registration\n");
		xie::CxFinalizer addr_finalizer([&addr]()
			{
				printf("(4) free\n");
				if (addr != NULL)
					xie::Axi::MemoryFree(addr);
				addr = NULL;
			});
		
		printf("(3) job\n");
		memset(addr, 0, 256*1);

		// Even if process end in the middle of scope, above finalize is called.
		throw xie::CxException(xie::ExStatus::Success, __FUNCTION__, __FILE__, __LINE__);
	}
	catch(const xie::CxException&)
	{
		printf("(5) exception\n");
	}
}

}
