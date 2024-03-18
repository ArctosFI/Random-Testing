Shader "Custom/Stencil"
{
    Properties
    {
        [IntRange] _stencilID ("Stencil ID", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "RenderPipeline"="UniveraslPipeline"
            "Queue"="Geometry"
        }
        
        Pass
        {
            Blend Zero One
            ZWrite Off

            Stencil
            {
                Ref [_stencilID]
                Comp Always
                Pass Replace
                Fail Keep
            }
        }
    }
}
