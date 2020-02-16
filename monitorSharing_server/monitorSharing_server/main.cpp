#include <iostream>
#include <atlimage.h>
#include <gdiplusbitmap.h>

#include <Windows.h>
#include <time.h>

using namespace std;

void ScreenCapture(int fileNum){
	HDC HandleDC = GetWindowDC(NULL);
	CImage imageObject;

	//::SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT_SYSTEM_AWARE);

	int cx = GetSystemMetrics(SM_CXSCREEN);
	int cy = GetSystemMetrics(SM_CYSCREEN);
	int colorDepth = GetDeviceCaps(HandleDC, BITSPIXEL);

	imageObject.Create(cx, cy, colorDepth);
	StretchBlt(imageObject.GetDC(), 0, 0, cx, cy, HandleDC, 0, 0, cx, cy, SRCCOPY);

	CString fileName = "C://monitorSharingFTP/ScreenCapture";
	CString fileType = ".jpeg";

	CString fileResult;
	fileResult.Format(_T("%s%d%s"), fileName, fileNum, fileType);

	imageObject.Save((LPCTSTR)fileResult, Gdiplus::ImageFormatJPEG);

	ReleaseDC(NULL, HandleDC);
	imageObject.ReleaseDC();
}

int main() {
	clock_t start, end;

	char input = NULL;
	cout << "> ";
	cin >> input;
	if (input == 's') {
		system("cls");

		while (1) {
			start = clock();
			for (int i = 0; i < 10; i++)	ScreenCapture(i);
			end = clock();

			cout << "Time : " << (double)(end - start) << endl;
			//Sleep(1000);
		}
	}

	return 0;
}