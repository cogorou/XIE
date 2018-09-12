/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXSERIAPORT_H_INCLUDED_
#define _TXSERIAPORT_H_INCLUDED_

#include "xie_high.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace IO
{

struct XIE_EXPORT_CLASS TxSerialPort
{
	int			BaudRate;
	ExParity	Parity;
	int			DataBits;
	ExStopBits	StopBits;
	ExHandshake	Handshake;

#if defined(__cplusplus)
	static inline TxSerialPort Default()
	{
		TxSerialPort result;
		result.BaudRate		= 9600;
		result.Parity		= ExParity::None;
		result.DataBits		= 8;
		result.StopBits		= ExStopBits::One;
		result.Handshake	= ExHandshake::None;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxSerialPort();
	TxSerialPort(int buadrate, ExParity parity, int databits, ExStopBits stopbits, ExHandshake handshake);

	bool operator == (const TxSerialPort& cmp) const;
	bool operator != (const TxSerialPort& cmp) const;
#endif
};

}
}

#pragma pack(pop)

#endif
