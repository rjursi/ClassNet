#include "server_socket_class.h"
#include <Windows.h>

void ScreenCapture() {
	HDC HandleDC = GetWindowDC(NULL);
	CImage imageObject;
	::SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT_SYSTEM_AWARE);
	// 윈도우 10에서 사용하기 위한 DPI 사용하기 위한 툴
	// 화면 비율을 맞춰주기 위한 것

	int cx = GetSystemMetrics(SM_CXSCREEN);
	int cy = GetSystemMetrics(SM_CYSCREEN);
	int colorDepth = GetDeviceCaps(HandleDC, BITSPIXEL);

	imageObject.Create(cx, cy, colorDepth, 0);
	BitBlt(imageObject.GetDC(), 0, 0, cx, cy, HandleDC, 0, 0, SRCCOPY);

	imageObject.Save(L"ScreenCapture.jpeg", Gdiplus::ImageFormatJPEG);

	ReleaseDC(NULL, HandleDC);
	imageObject.ReleaseDC();
}

// 서버의 화면 캡쳐하는 함수

int main() {
	server_socket_class server_socket_object;

	char c_input = NULL;
	//while (1) {
	cout << "> ";
	cin >> c_input;
	if (c_input == 's') {
		system("cls");
		while (1) {
			ScreenCapture();
			server_socket_object.sendfile();
		}
	}
	return 0;
}