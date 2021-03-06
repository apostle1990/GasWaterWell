clear
clc
tic
%% 功能：计算产水气井动态无阻流量
%% 参数输入（需要人为输入）
rhowsc=1000;%水的密度,kg/m3
rhogsc=0.693;%气体密度,kg/m3

S=-6.5;%气井表皮系数
D=4.0E-6;%非达西渗流系数
Re=1500;%井控半径
rw=0.1;%井径
Pi=76.05;%原始地层压力，MPa
%% 数据读入（Excel表格中的数据需要输入）
Pro=xlsread('压力与流体性质关系.xlsx','sheet1');
Kr=xlsread('气水相渗.xlsx','sheet1');
PrData=xlsread('生产数据.xlsx','sheet1');
%% 读入数据初步处理
PrData(isnan(PrData))=0;
P=Pro(:,1);%压力
rhog=Pro(:,2);%气体密度
miug=Pro(:,3);%气体粘度
rhow=Pro(:,4);%水密度
miuw=Pro(:,5);%水粘度
Bg=Pro(:,6);%气体体积系数
Bw=Pro(:,7);%水的体积系数
Sw=Kr(:,1);%水的相渗
Krg=Kr(:,2);%气体相对密度
Krw=Kr(:,3);%水的相对密度
Qgr=PrData(:,1);%实际气井产量
Qwr=PrData(:,2);%实际产水量
Pwf=PrData(:,3);%井底流压
Pe=PrData(:,4);%地层压力
Gp=PrData(:,5);%累积产气量
QWGR=Qwr./Qgr;%实际水气比,每万方
QWGR(isnan(QWGR))=0;%将水气比为NAN，即0/O转化为0
QWGR(isinf(QWGR))=0;%将水气比为inf，即x/O转化为0
Qwgr=QWGR*1E-4;
Qgr=Qgr*1E4;%转换为方
n=length(Qgr);
 %————————根据关系式计算地层压力———————————%
%% 参数输入判断
if min(Pe-Pwf)<0
    NN1=sum(Pe-Pwf<0);
    NN1=num2str(NN1);
    str1=strcat('有',NN1,'个井底流压大于地层压力，存在错误，请检查！');
    h1=msgbox(str1,'提示信息1','help','modal');
end
if max(Pwf)>Pi
    NN2=sum(Pwf-Pi>0);
    NN2=num2str(NN2);
    str2=strcat('有',NN2,'个井底流压大于原始地层压力，存在错误，请检查！');
%     Hang=Index(1);
%     Lie=Index(2);
    h2=msgbox(str2,'提示信息2','help','modal');
end
% 井底流压和地层压力互换
Index=find(Pe-Pwf<0);%确定小于0的下标
Tem=Pe(Index);
Pe(Index)=Pwf(Index);
Pwf(Index)=Tem;
%% 参数处理
%——————确定krw，krg，Sw等与压力之间的关系式————————%
kwratio=Krw./Krg;%水相与气相相对渗透率之比
maxRwg=max(QWGR)+1;%确保最大水气比可以插值获得
Rwg=0:0.1:maxRwg;%设定一系列水气比,当水气比为0，则Krg=Krg(end)
KrwKrg=((Bw.*miuw)./(Bg.*miug))*Rwg*1E-4;%不同的行代表不同的压力，不同的列代表不同的水气比
[rn,cn]=size(KrwKrg);%rn代表行数，表征不同的压力，cn代表列数，表征不同的水气比
KrwP=zeros(rn,cn);
KrgP=zeros(rn,cn);
for i=1:rn
    for j=1:cn
        KrwP(i,j)=interp1(kwratio,Krw,KrwKrg(i,j));
        KrgP(i,j)=interp1(kwratio,Krg,KrwKrg(i,j));
    end
end
%————————数值积分过程————————%
PSTa=integra(KrgP,KrwP,miug,miuw,rhog,rhow,P);%拟压力数值表
%————————计算累产气——————————%
% Gp=zeros(1,n);
% for i=1:n
%     Gp(i)=sum(Qgr(1:i));
% end
%% 根据实际生产数据，基于得到的拟压力函数表，插值获得一系列的拟压力值，并计算动态无阻流量
%——————————计算A、B系数值—————————————%
A1=1.8665*(rhogsc+rhowsc*Qwgr')*(log(Re/rw)-0.75+S);
B1=1.8665*(rhogsc+rhowsc*Qwgr').^2*D;
AG1=1.8665*rhogsc*(log(Re/rw)-0.75+S);
BG1=1.8665*rhogsc^2*D;
KH=zeros(1,n);
PeS=zeros(1,n);%每一天的地层压力对应的拟压力
PwfS=zeros(1,n);%每一天的井底流压对应的拟压力
PeGS=zeros(1,n);%不考虑产水每一天的地层拟压力
for k=1:n
    %% 计算不同水气比KH
    %—————————先根据水气比进行插值——————————%
    PS=zeros(1,rn);
    for i=1:rn
        PS(i)=interp1(Rwg,PSTa(i,:),QWGR(k));%返回给定气水比对应的拟压力
    end
    %——————根据压力与拟压力之间的关系进行插值——————%
    
    PeS(k)=interp1(P,PS,Pe(k));
    PwfS(k)=interp1(P,PS,Pwf(k));
    PeGS(k)=interp1(P,PSTa(:,1),Pe(k));
    KH(k)=(A1(k)*Qgr(k)+B1(k)*Qgr(k)^2)/(PeS(k)-PwfS(k));%确定地层系数,mD,m
end
deltaPSSR=PeS;
deltaPSSRO=PeGS;
KHC=mean(KH(KH(1:30)>0));%不考虑产水，KH恒定,取前30天的平均KH值
A=A1./KH;
B=B1./KH;
AG=AG1/KHC;
BG=BG1/KHC;
QgwAOF=QAOF(A,B,deltaPSSR,0);%不同水气比对应的产气量,万方
QgAOF=QAOF(AG,BG,deltaPSSRO,0);%不考虑产水影响的动态无阻流量，万方
% QgwAOF(isnan(QgwAOF))=0;%将停产时间对应的无阻流量设为0
% QgAOF(isnan(QgAOF))=0;
A(isinf(A))=nan;
B(isinf(B))=nan;
%% 绘图
plot(1:n,QgwAOF,'ro')
hold on
plot(1:n,QgAOF)
%% 数据输出
Output=[QgwAOF',QgAOF',KH',A',B'];
Title={'考虑产水无阻流量/104m3','不考虑产水无阻流量/104m3','KH/(mD.m)','A','B'};
xlswrite('处理结果.xlsx',' ','Sheet1','A1:H10000')%将原来表格中数据清除，以写入新的数据
xlswrite('处理结果.xlsx',Title,'Sheet1','A1:E1')
on=n+1;
on=num2str(on);
cellnam=strcat('A2:','E',on);
xlswrite('处理结果.xlsx',Output,'sheet1',cellnam)
toc

    





