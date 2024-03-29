﻿using System.ComponentModel;
using BeautyControl.API.Features.Reports._Common;

namespace BeautyControl.API.Features.Reports.GetStockWorkflow
{
    [DisplayName("GetProductsWorkflowRequest")]
    public record GetProductsWorkflowQuery(DateTime? Start, DateTime? End)
    {
        // Mesmo que seja um DTO, tudo bem este método aqui por ser por uma questão bem específica dele não extrapolando nenhum compartamento ou "boa prática"
        public bool HasParams() => Start.HasValue || End.HasValue;
    };

    public record ProductWorkflowResponse(
        IEnumerable<ProductWorkflowDataResponse> Inputs,
        IEnumerable<ProductWorkflowDataResponse> Outputs
    );
}
