#ifndef CLIENTSOCKETCLASS_H
#define CLIENTSOCKETCLASS_H

#include <iostream>
#include <WinSock2.h>
#include <ws2tcpip.h>

#pragma	comment(lib, "ws2_32")

#define BUFSIZE 65000
#define PORT 9000

using namespace std;

class client_socket_class {
private:
	int temp;

	// ���� ���� ����
	WSADATA wsaData;
	SOCKET sock;
	SOCKADDR_IN sock_addr;
	// 

	BOOL broadcast_enable = TRUE;

	// �ϴ��� ������ ���� ����
	SOCKADDR_IN sender_addr;
	int sender_addr_size = sizeof(sender_addr);

	char var_connect[2];
	
	// ���� ������ ���ۿ� ����� ����
	char *totalbuf;
	char *buf;

	FILE *filepointer;

	int file_size;
	int recv_size = 0;
	int total_size;

	// ���� ����
	char msgbuf[2] = ".";

public:
	//������
	client_socket_class();
	//�Ҹ���
	~client_socket_class();

	// ���� ����
	void connect_server();

	//���� ����
	void recvfile();
};

#endif // !CLIENTSOCKETCLASS_H