function Qg=QAOF(A,B,PeS,PwfS)
%% ������������
deltaPs=PeS-PwfS;
Qg=(-A+(A.^2+4*B.*deltaPs).^0.5)./(2*B);
Qg=Qg/1E4;%����Ϊ��
end