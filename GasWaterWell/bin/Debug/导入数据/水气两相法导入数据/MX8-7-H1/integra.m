function V=integra(krg,krw,miug,miuw,rhog,rhow,P)
%% �������η�������ֵ����
[rn,cn]=size(krg);%rnΪ������cnΪ����
miug=repmat(miug,1,cn);
miuw=repmat(miuw,1,cn);
rhog=repmat(rhog,1,cn);
rhow=repmat(rhow,1,cn);
S=krg.*rhog./miug+krw.*rhow./miuw;
V=zeros(rn,cn);
for i=1:rn
    if i==1
        V(i,:)=S(i,:)*P(i);
    else 
        V(i,:)=V(i-1,:)+(S(i-1,:)+S(i,:))*(P(i)-P(i-1))/2;
    end
end
end