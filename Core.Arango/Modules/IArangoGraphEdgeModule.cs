﻿using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Graph edge manipulation
    /// </summary>
    public interface IArangoGraphEdgeModule
    {
        /// <summary>
        ///     Fetch an edge
        /// </summary>
        ValueTask<TR> GetAsync<TR>(ArangoHandle database, string graph, string collection, string key,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Creates an edge in an existing graph
        /// </summary>
        ValueTask<ArangoEdgeResponse<ArangoVoid>> CreateAsync<T>(ArangoHandle database, string graph, string collection,
            T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Modify an existing edge
        /// </summary>
        ValueTask<ArangoEdgeResponse<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replace the content of an existing edge
        /// </summary>
        ValueTask<ArangoEdgeResponse<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes an edge from graph
        /// </summary>
        ValueTask<ArangoEdgeResponse<ArangoVoid>> RemoveAsync<T>(ArangoHandle database, string graph, string collection,
            string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Creates an edge in an existing graph
        /// </summary>
        ValueTask<ArangoEdgeResponse<TR>> CreateAsync<T, TR>(ArangoHandle database, string graph, string collection, T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Modify an existing edge
        /// </summary>
        ValueTask<ArangoEdgeResponse<TR>> UpdateAsync<T, TR>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replace the content of an existing edge
        /// </summary>
        ValueTask<ArangoEdgeResponse<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes an edge from graph
        /// </summary>
        ValueTask<ArangoEdgeResponse<TR>> RemoveAsync<T, TR>(ArangoHandle database, string graph, string collection,
            string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);
    }
}