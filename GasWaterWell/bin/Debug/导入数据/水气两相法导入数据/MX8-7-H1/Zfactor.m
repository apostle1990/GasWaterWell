function Z=Zfactor(P,T,Pc,Tc)
Pr=P/Pc;
Tr=T/Tc;
t=1/Tr;
%% 牛顿迭代求y
eps=1E-6;
y0=0;%初始迭代值
fy=-0.06125*t*exp(-1.2*(1-t)^2)*Pr+(y0+y0^2+y0^3-y0^4)/(1-y0)^3......
    -(14.76*t-9.76*t^2+4.58*t^3)*y0^2+(90.7*t-242.2*t^2+42.4*t^3)*y0^(2.18+2.82*t);
dfy=(1+4*y0+4*y0^2-4*y0^3+4*y0)/(1-y0)^4-(29.52*t-19.52*t^2+9.16*t^3)*y0.....
    +(2.18+2.82*t)*(90.7*t-242.2*t^2+42.4*t^3)*y0^(1.18+2.82*t);
y1=y0-fy/dfy;
err=abs(y0-y1);
i=1;
while err>eps
    y0=y1;
    fy=-0.06125*t*exp(-1.2*(1-t)^2)*Pr+(y0+y0^2+y0^3-y0^4)/(1-y0)^3......
    -(14.76*t-9.76*t^2+4.58*t^3)*y0^2+(90.7*t-242.2*t^2+42.4*t^3)*y0^(2.18+2.82*t);
    dfy=(1+4*y0+4*y0^2-4*y0^3+4*y0)/(1-y0)^4-(29.52*t-19.52*t^2+9.16*t^3)*y0.....
        +(2.18+2.82*t)*(90.7*t-242.2*t^2+42.4*t^3)*y0^(1.18+2.82*t);
    y1=y0-fy/dfy;
    err=abs(y0-y1);
    i=i+1;
end
Z=(0.06125*t)*exp(-1.2*(1-t)^2)*(Pr/y0);
end