using System; // 기본 시스템 기능
using System.Collections.Generic; // 제네릭 컬렉션
using System.Linq; // LINQ 확장
using System.Threading.Tasks; // 비동기 지원
using System.Windows.Forms; // WinForms UI 지원

namespace SLC_Controller {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() { // 애플리케이션 시작 지점
            Application.EnableVisualStyles(); // 최신 비주얼 스타일 적용
            Application.SetCompatibleTextRenderingDefault(false); // 텍스트 렌더링 설정
            Application.Run(new Form1()); // 메인 폼 실행
        }
    }
}

