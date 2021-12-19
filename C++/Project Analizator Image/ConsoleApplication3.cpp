
#include "Header.h"
using namespace std;
unsigned int histogram[256];
int alphabet[256];
int main(int argc, char** args)
{
	fstream f;
	ofstream out("histogram.txt");
	char PGM_hdr1[2];
	int width, heidth;
	width = heidth = 0;
	if (argc == 1)
	{
		cout << "Help" << endl;
		return 1;
	}
	for (int i = 1;i < argc;i++)
	{
		f.open(args[i], ios::binary | ios::in);
		if (f.is_open())
		{
			cout << "Help file not open" << endl;
			continue;
		}
		cout << "file open" << endl;
		f.read(PGM_hdr1,2);
		if ((PGM_hdr1[0] != 0x50) || (PGM_hdr1[1] != 0x35)) continue;
		cout << "file read" << endl;
		f.seekg(static_cast<unsigned int>(f.tellg())+ 1);
		unsigned int digit_b = static_cast<unsigned int>(f.tellg());
		char sym;
		do
		{
			f.read(&sym,1);
		} while (sym != 0x20);
		unsigned int digit_e = static_cast<unsigned int>(f.tellg());
		int j = 0;
		for (int d = '0';d <= '9';d++, j++) alphabet[d] = j;

		int c = 0;
		for (j = digit_e;j >= digit_b;j--, c *= 10)
		{
			f.seekg(j);
			f.read(&sym, 1);
			width += alphabet[sym] * c;
		}
		cout << "Width: "<< width << endl;

		digit_b = digit_e + 2;
		c = 1;
		do
		{
			f.read(&sym, 1);
		} while(sym!=0x0A);
		digit_e = static_cast<unsigned int>(f.tellg()) - 2;
		for (j = digit_e;j >= digit_b;j--, c *= 10)
		{
			f.seekg(j);
			f.read(&sym, 1);
			heidth += alphabet[sym] * c;
		}
		cout << "Heidth " << width << endl;
		for (int d = 0;d < 256;++d) histogram[d] = 0;
		f.seekg(digit_e + 6);
		for (int d = 0;d < (width + heidth);++d)
		{
			if (f.eof())break;
			f.read(&sym, 1);
			histogram[static_cast<unsigned char>(sym)] += 1;
		}
		for (j = 0;j < 256;j++)
		{
			cout << "[" << j << "}  ";
			for (c = 0;c < histogram[j];c++)
				cout << "|";
			cout << endl;
		}
		f.close();
		out << "File: " << args[i] << endl;
		out << "Width: " << width << "\t Height: " << heidth << endl;
		for (j = 0;j < 256;j++)
		{
			out << "[" << j << "}  ";
			for (c = 0;c < histogram[j];c++)
				out << "|";
			out << endl;
		}
		out << "End of file" << args[i] << endl << endl;
		out.close();
	}
	return 0;
}

// Запуск программы: CTRL+F5 или меню "Отладка" > "Запуск без отладки"
// Отладка программы: F5 или меню "Отладка" > "Запустить отладку"

// Советы по началу работы 
//   1. В окне обозревателя решений можно добавлять файлы и управлять ими.
//   2. В окне Team Explorer можно подключиться к системе управления версиями.
//   3. В окне "Выходные данные" можно просматривать выходные данные сборки и другие сообщения.
//   4. В окне "Список ошибок" можно просматривать ошибки.
//   5. Последовательно выберите пункты меню "Проект" > "Добавить новый элемент", чтобы создать файлы кода, или "Проект" > "Добавить существующий элемент", чтобы добавить в проект существующие файлы кода.
//   6. Чтобы снова открыть этот проект позже, выберите пункты меню "Файл" > "Открыть" > "Проект" и выберите SLN-файл.
