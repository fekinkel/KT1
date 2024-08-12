unit usenha;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, Buttons, ExtCtrls, Math, jpeg
  , uGlobal;

type
  TForm1 = class(TForm)
    edtSenha: TEdit;
    btnOk: TBitBtn;
    btnCancela: TBitBtn;
    edtUsuario: TEdit;
    Edit1: TEdit;
    procedure btnOkClick(Sender: TObject);
    function fGerarSenha(S: String): String;
    procedure btnCancelaClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    function LogUser : String;
    procedure edtUsuarioChange(Sender: TObject);
    procedure FormKeyDown(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure edtUsuarioEnter(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.DFM}

procedure TForm1.btnOkClick(Sender: TObject);
begin
  edtSenha.text := fgerarsenha(edtUsuario.text);
  edtUsuario.SelectAll;
//  edtSenha.SetFocus;
end;

{ Encriptação de uma string }
function TForm1.fGerarSenha(S: String): String;
var
  i : Integer;
  vlAno, vlMes, vlDia: Word;
  vlData:string;
begin
  s := UpperCase(s)+'ABCDEFGH';
  result  := '';
  DecodeDate(date, vlANo, vlMes, vlDia);
  vlData:= IntToStr(vlAno)+IntToStr(vlMes)+IntToStr(vlDia);
  for i:= 1 to 8 do begin
    Result:= Result + IntToStr( (777+3*I)-(Ord(vlData[9-i]) + Ord(s[i])))[3]
  end;
  vlData  := Result;
  result:= '';
  for i:=1 to 8 do if i<>4 then result:=result +vlData[i] else result:=result +vlData[i]+'-'
end;

procedure TForm1.btnCancelaClick(Sender: TObject);
begin
  Application.Terminate;
end;


procedure TForm1.FormCreate(Sender: TObject);
begin
//  Caption := Caption + ' [versão 0503.10]';
  edtUsuario.Text := LogUser();
  edit1.Text :=DateToStr(Date);
  btnOk.click;
end;

function TForm1.LogUser : String;
begin
  result:=vgNomeLogin;
end;


procedure TForm1.edtUsuarioChange(Sender: TObject);
begin
  if trim(TEdit(Sender).Text)=''
  then edtSenha.text := ''
  else edtSenha.text := fgerarsenha(TEdit(Sender).text);

end;

procedure TForm1.edtUsuarioEnter(Sender: TObject);
begin
//  btnOk.Click;
end;

procedure TForm1.FormKeyDown(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
  if key = Vk_Escape then
    Application.Terminate;
end;

end.
