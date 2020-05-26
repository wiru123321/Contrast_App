.code
operateOnPixelsAsm PROC 

	movdqu xmm1, [rbx + 0*SIZEOF BYTE]	;load vector LUT

	movdqu [rcx],xmm1	 ; Store vector LUT in pixels array

	ret								
operateOnPixelsAsm ENDP
END									