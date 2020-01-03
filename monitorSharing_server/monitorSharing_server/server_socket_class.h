#ifndef SERVERSOCKETCLASS_H
#define SERVERSOCKETCLASS_H

#include <iostream>
#include <WinSock2.h>
#include <ws2tcpip.h>

#include <atlImage.h>

#pragma	comment(lib, "ws2_32")

#define BUFSIZE 65000
#define PORT 7777

using namespace std;

class server_socket_class {
private:
	// 소켓 관련 변수
	WSADATA wsaData;
	SOCKET sock;
	SOCKADDR_IN sock_addr;

	BOOL broadcast_enable = TRUE;

	// 파일 데이터 전송에 사용할 변수
	char *buf;

	FILE *filepointer;
	int file_size;
	int send_size;

	// 응답 변수
	SOCKADDR_IN recver_addr;
	int recver_addr_size = sizeof(recver_addr);
	char msgbuf[2];

public:
	// 생성자
	server_socket_class();
	// 소멸자
	~server_socket_class();

	// 파일전송
	void sendfile();
};

#endif // !SERVERSOCKETCLASS_H