#include "server_socket_class.h"
#include <time.h>

server_socket_class::server_socket_class() {
	WSAStartup(MAKEWORD(2, 2), &wsaData);

	/// ������ �����ϴ� Word ũ���, 0000001000000010 =  514
	/// wsaData : Window ���� ������ ���λ����� �����ϴ� WSADATA ������ ���� ������



	sock = socket(PF_INET, SOCK_DGRAM, IPPROTO_UDP);
	// PF_INET - IPv4 �ǹ�, 
	// SOCK_DGRAM - ������ �׷� ���� ����
	// IPPROTO_UDP - UDP ���� ����


	setsockopt(sock, SOL_SOCKET, SO_BROADCAST, (char*)&broadcast_enable, sizeof(broadcast_enable));

	// �ɼ��� ������ ����, SOL_SOCKET, ���� �����̴�. ���� �������� ������ �ɼ��� �����ϰڴ�.

	// broadcast �ɼ� TRUE ����

	memset(&sock_addr, 0, sizeof(sock_addr));
	// ���� ������ ��� ����ü �ʱ�ȭ

	sock_addr.sin_family = AF_INET;
	sock_addr.sin_addr.s_addr =  htonl(INADDR_BROADCAST); // inet_addr("192.168.31.200");
	/// ������ �ּҴ� ��ε� ĳ��Ʈ �ּ�


	sock_addr.sin_port = htons(PORT);
}

server_socket_class::~server_socket_class() {
	closesocket(sock);
	WSACleanup();
}

void server_socket_class::connect_client() {
	sendto(sock, (char*)&var_request, sizeof(var_request), 0, (const SOCKADDR*)&sock_addr, sizeof(sock_addr));

	// ���ۿ� �ִ� ������ ��ŭ ����, �� �������� ���� (Broadcast)


	ZeroMemory(var_response, sizeof(var_response));
	// ���� ���۸� ���


	recvfrom(sock, var_response, sizeof(var_response), 0, (SOCKADDR*)&recver_addr, &recver_addr_size);
	// �������κ��� ������ �����͸� �ش� var_response ���ۿ� ����, �ش� ���� Ŭ���̾�Ʈ ������ recver_addr �� ����

}	

void server_socket_class::sendfile() {
	clock_t start, end;

	// ���� ����
	filepointer = fopen("ScreenCapture.jpeg", "rb");
	fseek(filepointer, 0, SEEK_END); // ���� ������ ��ġ�� �̵�


	file_size = ftell(filepointer);
	// ���� �������� ���� ��ġ�� ��ȯ


	rewind(filepointer);
	// ���� �����͸� ���� ������ �̵�


	sendto(sock, (char*)&file_size, sizeof(file_size), 0, (const SOCKADDR*)&recver_addr, sizeof(recver_addr));
	/// ���� ũ�⿡ ���� ������ ����

	// �ش� ������ �����͸� recver_addr ���� ������ ����

	cout << "���� ũ�� : " << file_size << " Byte" << endl;

	cout << "\nUploading ";

	start = clock();
	// ���� ����
	buf = new char[BUFSIZE];


	while (1) {
		ZeroMemory(buf, _msize(buf));

		// _msize : ���� �Ҵ��� ������ ���Ͽ� ũ�⸦ �˾ƿ��� �Լ�

		send_size = fread(buf, 1, _msize(buf), filepointer);
		/// filepointer ���� �� _msize(buf) ���� 1����Ʈ�� �о�´�.
		/// buf �� ���� ���� ���ۿ��� ��
		sendto(sock, buf, _msize(buf), 0, (SOCKADDR*)&recver_addr, sizeof(recver_addr));

		// ������ ������ ��� ���۸� ����
		// �� ������ ����



		// ���� ����
		recvfrom(sock, msgbuf, sizeof(msgbuf), 0, NULL, 0);
		cout << msgbuf << " ";

		file_size -= send_size;
		if (file_size == 0) break;

		// ������ ����
		if (file_size < BUFSIZE) {
			delete[] buf;
			buf = NULL;
			buf = new char[file_size];
		}


		/// ���� ���� ũ�⿡ �°� ���۸� �ٽ� �����ϰ� ����
		/// ���� �õ�

	}

	end = clock();
	fclose(filepointer);

	delete[] buf;
	buf = NULL;

	// ���� ���
	cout << "\n\n�������� ���� (Time : " << (double)(end - start) << ")\n" << endl;
}