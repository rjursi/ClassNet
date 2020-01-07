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

	// 소켓 관련 변수
	WSADATA wsaData;
	SOCKET sock;
	SOCKADDR_IN sock_addr;
	// 

	BOOL broadcast_enable = TRUE;

	// 일대일 연결을 위한 변수
	SOCKADDR_IN sender_addr;
	int sender_addr_size = sizeof(sender_addr);

	char var_connect[2];
	
	// 파일 데이터 전송에 사용할 변수
	char *totalbuf;
	char *buf;

	FILE *filepointer;

	int file_size;
	int recv_size = 0;
	int total_size;

	// 응답 변수
	char msgbuf[2] = ".";

public:
	//생성자
	client_socket_class();
	//소멸자
	~client_socket_class();

	// 서버 연결
	void connect_server();

	//파일 수신
	void recvfile();
};

#endif // !CLIENTSOCKETCLASS_H